using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Interfaces;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using ClosedXML.Excel;
using Microsoft.Extensions.Logging;

namespace Bookong.Application.UseCases
{
    public class ImportBooksUseCase : IImportBooksUseCase
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<ImportBooksUseCase> _logger;

        private const string DefaultGenreName = "Nezařazeno";
        private const string DefaultKindName = "Nezařazeno";
        private const string DefaultPeriodName = "Nezařazeno";
        private const string DefaultWarehouseName = "Hlavní sklad";

        public ImportBooksUseCase(IUnitOfWork uow, ILogger<ImportBooksUseCase> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<GenericResponse> ImportFromExcelAsync(Stream fileStream, string fileName)
        {
            if (fileStream == null || fileStream.Length == 0)
            {
                return GenericResponse.FailureResponse("Nebyl vybrán žádný soubor.");
            }

            if (!fileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) && 
                !fileName.EndsWith(".xls", StringComparison.OrdinalIgnoreCase))
            {
                return GenericResponse.FailureResponse("Soubor musí být ve formátu Excel (.xlsx nebo .xls).");
            }

            try
            {
                var importedBooksData = await ParseExcelFileAsync(fileStream);

                if (importedBooksData.Count == 0)
                {
                    return GenericResponse.FailureResponse("V souboru nebyly nalezeny žádné platné knihy.");
                }

                _logger.LogInformation("📊 Načteno {Count} knih z excelu", importedBooksData.Count);
                _logger.LogInformation("📊 Z toho má Public ID: {WithId}, bez Public ID: {WithoutId}", 
                    importedBooksData.Count(b => b.PublicId.HasValue),
                    importedBooksData.Count(b => !b.PublicId.HasValue));

                var existingBooks = (await _uow.Books.GetAllAsync()).ToList();

                _logger.LogInformation("📚 V databázi je celkem {Count} knih", existingBooks.Count);
                var authors = (await _uow.Authors.GetAllAsync()).ToList();
                var genres = (await _uow.Genres.GetAllAsync()).ToList();
                var kinds = (await _uow.Kinds.GetAllAsync()).ToList();
                var periods = (await _uow.Periods.GetAllAsync()).ToList();
                var warehouses = (await _uow.Warehouses.GetAllAsync()).ToList();

                bool needsCommit = false;

                CreateGenreIfNotExists(DefaultGenreName, genres, ref needsCommit);
                CreateKindIfNotExists(DefaultKindName, kinds, ref needsCommit);
                CreatePeriodIfNotExists(DefaultPeriodName, periods, ref needsCommit);
                CreateWarehouseIfNotExists(DefaultWarehouseName, warehouses, ref needsCommit);

                foreach (var bookData in importedBooksData)
                {
                    CreateAuthorIfNotExists(bookData.AuthorFirstName, bookData.AuthorLastName, authors, ref needsCommit);
                    CreateGenreIfNotExists(bookData.Genre, genres, ref needsCommit);
                    CreateKindIfNotExists(bookData.Kind, kinds, ref needsCommit);
                    CreatePeriodIfNotExists(bookData.Period, periods, ref needsCommit);
                    CreateWarehouseIfNotExists(bookData.Warehouse, warehouses, ref needsCommit);
                }

                if (needsCommit)
                {
                    await _uow.CommitAsync();

                    existingBooks = (await _uow.Books.GetAllAsync()).ToList();
                    authors = (await _uow.Authors.GetAllAsync()).ToList();
                    genres = (await _uow.Genres.GetAllAsync()).ToList();
                    kinds = (await _uow.Kinds.GetAllAsync()).ToList();
                    periods = (await _uow.Periods.GetAllAsync()).ToList();
                    warehouses = (await _uow.Warehouses.GetAllAsync()).ToList();
                }

                int successCount = 0;
                int errorCount = 0;
                int skippedCount = 0;

                foreach (var bookData in importedBooksData)
                {
                    try
                    {
                        if (bookData.PublicId.HasValue && existingBooks.Any(b => b.PublicId == bookData.PublicId.Value))
                        {
                            skippedCount++;
                            continue;
                        }

                        if (!bookData.PublicId.HasValue)
                        {
                            _logger.LogWarning("Kniha {BookName} nemá Public ID v excelu - bude vytvořeno nové", bookData.Name);
                        }

                        var author = FindAuthor(bookData.AuthorFirstName, bookData.AuthorLastName, authors);
                        var genre = FindGenre(bookData.Genre, genres);
                        var kind = FindKind(bookData.Kind, kinds);
                        var period = FindPeriod(bookData.Period, periods);
                        var warehouse = FindWarehouse(bookData.Warehouse, warehouses);

                        DateTime? dateRelease = null;
                        if (bookData.DateRelease.HasValue)
                        {
                            dateRelease = bookData.DateRelease.Value;
                        }

                        var book = new Book
                        {
                            PublicId = bookData.PublicId ?? Guid.NewGuid(),
                            Name = bookData.Name,
                            ISBN = bookData.ISBN,
                            AuthorId = author?.Id,
                            GenreId = genre?.Id,
                            KindId = kind?.Id,
                            PeriodId = period?.Id,
                            WarehouseId = warehouse?.Id,
                            DateRelease = dateRelease,
                            Pages = bookData.Pages > 0 ? (ushort?)bookData.Pages : null,
                            Borrowable = true
                        };

                        _uow.Books.Add(book);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        _logger.LogError(ex, "Chyba při vytváření knihy: {BookName}", bookData.Name);
                    }
                }

                await _uow.CommitAsync();

                _logger.LogInformation("Import dokončen: {SuccessCount} knih importováno, {SkippedCount} přeskočeno, {ErrorCount} chyb", successCount, skippedCount, errorCount);

                var message = $"Import dokončen! Úspěšně importováno {successCount} knih.";

                if (skippedCount > 0)
                {
                    message += $" Přeskočeno duplicit: {skippedCount}.";
                }

                if (errorCount > 0)
                {
                    message += $" Chyb: {errorCount}.";
                }

                return GenericResponse.SuccessResponse(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Chyba při importu souboru {FileName}", fileName);
                return GenericResponse.FailureResponse($"Chyba při zpracování souboru: {ex.Message}");
            }
        }

        private async Task<List<ImportedBookData>> ParseExcelFileAsync(Stream fileStream)
        {
            var importedBooksData = new List<ImportedBookData>();

            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            using var workbook = new XLWorkbook(memoryStream);
            var worksheet = workbook.Worksheet(1);
            var rows = worksheet.RangeUsed().RowsUsed().Skip(1);

            int rowNumber = 2;
            foreach (var row in rows)
            {
                try
                {
                    var name = row.Cell(1).GetValue<string>();
                    var authorFullName = row.Cell(2).GetValue<string>();
                    var isbn = row.Cell(3).GetValue<string>();
                    var genre = row.Cell(4).GetValue<string>();
                    var kind = row.Cell(5).GetValue<string>();
                    var period = row.Cell(6).GetValue<string>();
                    var pagesStr = row.Cell(7).GetValue<string>();
                    var dateStr = row.Cell(8).GetValue<string>();
                    var warehouse = row.Cell(9).GetValue<string>();
                    var publicIdStr = row.Cell(11).GetValue<string>();

                    var (firstName, lastName) = SplitAuthorName(authorFullName);

                    int pages = 0;
                    if (!string.IsNullOrWhiteSpace(pagesStr) && int.TryParse(pagesStr, out var p))
                    {
                        pages = p;
                    }

                    DateTime? dateRelease = null;
                    if (!string.IsNullOrWhiteSpace(dateStr) && dateStr != "Neznámé datum")
                    {
                        if (DateTime.TryParseExact(dateStr, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out var date))
                        {
                            dateRelease = date;
                        }
                    }

                    Guid? publicId = null;
                    if (!string.IsNullOrWhiteSpace(publicIdStr) && Guid.TryParse(publicIdStr, out var guid))
                    {
                        publicId = guid;
                    }

                    var bookData = new ImportedBookData
                    {
                        Name = string.IsNullOrWhiteSpace(name) || name == "Bez názvu" ? "Unknown" : name.Trim(),
                        AuthorFirstName = firstName,
                        AuthorLastName = lastName,
                        ISBN = string.IsNullOrWhiteSpace(isbn) || isbn == "Bez ISBN" ? null : isbn.Trim(),
                        Genre = string.IsNullOrWhiteSpace(genre) || genre == "Nezařazeno" ? DefaultGenreName : genre.Trim(),
                        Kind = string.IsNullOrWhiteSpace(kind) || kind == "Nezařazeno" ? DefaultKindName : kind.Trim(),
                        Period = string.IsNullOrWhiteSpace(period) || period == "Nezařazeno" ? DefaultPeriodName : period.Trim(),
                        Warehouse = string.IsNullOrWhiteSpace(warehouse) || warehouse == "Nezařazeno" ? DefaultWarehouseName : warehouse.Trim(),
                        Pages = pages,
                        DateRelease = dateRelease,
                        PublicId = publicId
                    };

                    importedBooksData.Add(bookData);
                    rowNumber++;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Chyba při čtení řádku {RowNumber}", rowNumber);
                    rowNumber++;
                }
            }

            return importedBooksData;
        }

        private (string firstName, string lastName) SplitAuthorName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName) || fullName == "Neznámý autor")
            {
                return ("Unknown", "Author");
            }

            var parts = fullName.Trim().Split(' ', 2);
            if (parts.Length == 2)
            {
                return (parts[0], parts[1]);
            }
            else if (parts.Length == 1)
            {
                return (string.Empty, parts[0]);
            }

            return ("Unknown", "Author");
        }

        private void CreateAuthorIfNotExists(string firstName, string lastName, List<Author> authors, ref bool needsCommit)
        {
            if (!authors.Any(a => a.FirstName == firstName && a.LastName == lastName))
            {
                var author = new Author 
                { 
                    PublicId = Guid.NewGuid(),
                    FirstName = firstName, 
                    MiddleName = "",
                    LastName = lastName 
                };
                _uow.Authors.Add(author);
                authors.Add(author);
                needsCommit = true;
            }
        }

        private void CreateWarehouseIfNotExists(string warehouseName, List<Warehouse> warehouses, ref bool needsCommit)
        {
            if (!warehouses.Any(w => w.Name == warehouseName))
            {
                var warehouse = new Warehouse 
                { 
                    PublicId = Guid.NewGuid(),
                    Name = warehouseName,
                    Address = new Address
                    {
                        PublicId = Guid.NewGuid(),
                        Street = "Neznámá",
                        Number = "0",
                        City = "Neznámé",
                        ZipCode = "00000"
                    }
                };
                _uow.Warehouses.Add(warehouse);
                warehouses.Add(warehouse);
                needsCommit = true;
            }
        }

        private void CreateGenreIfNotExists(string genreName, List<Genre> genres, ref bool needsCommit)
        {
            if (!genres.Any(g => g.Name == genreName))
            {
                var genre = new Genre 
                { 
                    PublicId = Guid.NewGuid(),
                    Name = genreName 
                };
                _uow.Genres.Add(genre);
                genres.Add(genre);
                needsCommit = true;
            }
        }

        private void CreateKindIfNotExists(string kindName, List<Kind> kinds, ref bool needsCommit)
        {
            if (!kinds.Any(k => k.Name == kindName))
            {
                var kind = new Kind 
                { 
                    PublicId = Guid.NewGuid(),
                    Name = kindName 
                };
                _uow.Kinds.Add(kind);
                kinds.Add(kind);
                needsCommit = true;
            }
        }

        private void CreatePeriodIfNotExists(string periodName, List<Period> periods, ref bool needsCommit)
        {
            if (!periods.Any(p => p.Name == periodName))
            {
                var period = new Period 
                { 
                    PublicId = Guid.NewGuid(),
                    Name = periodName 
                };
                _uow.Periods.Add(period);
                periods.Add(period);
                needsCommit = true;
            }
        }

        private Author? FindAuthor(string firstName, string lastName, IEnumerable<Author> authors)
        {
            return authors.FirstOrDefault(a => a.FirstName == firstName && a.LastName == lastName);
        }

        private Genre? FindGenre(string genreName, IEnumerable<Genre> genres)
        {
            return genres.FirstOrDefault(g => g.Name == genreName);
        }

        private Kind? FindKind(string kindName, IEnumerable<Kind> kinds)
        {
            return kinds.FirstOrDefault(k => k.Name == kindName);
        }

        private Period? FindPeriod(string periodName, IEnumerable<Period> periods)
        {
            return periods.FirstOrDefault(p => p.Name == periodName);
        }

        private Warehouse? FindWarehouse(string warehouseName, IEnumerable<Warehouse> warehouses)
        {
            return warehouses.FirstOrDefault(w => w.Name == warehouseName);
        }

        private record ImportedBookData
        {
            public string Name { get; init; } = "";
            public string AuthorFirstName { get; init; } = "";
            public string AuthorLastName { get; init; } = "";
            public string? ISBN { get; init; }
            public string Genre { get; init; } = "";
            public string Kind { get; init; } = "";
            public string Period { get; init; } = "";
            public string Warehouse { get; init; } = "";
            public int Pages { get; init; }
            public DateTime? DateRelease { get; init; }
            public Guid? PublicId { get; init; }
        }
    }
}
