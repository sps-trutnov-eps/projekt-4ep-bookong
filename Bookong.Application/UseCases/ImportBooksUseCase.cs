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

                var authors = await _uow.Import.GetAllAuthorsAsync();
                var genres = await _uow.Import.GetAllGenresAsync();
                var kinds = await _uow.Import.GetAllKindsAsync();
                var periods = await _uow.Import.GetAllPeriodsAsync();
                var warehouses = await _uow.Import.GetAllWarehousesAsync();

                bool needsCommit = false;

                foreach (var bookData in importedBooksData)
                {
                    _uow.Import.CreateAuthorIfNotExists(bookData.Author, authors, ref needsCommit);
                    _uow.Import.CreateGenreIfNotExists(bookData.Genre, genres, ref needsCommit);
                    _uow.Import.CreateKindIfNotExists(bookData.Kind, kinds, ref needsCommit);
                    _uow.Import.CreatePeriodIfNotExists(bookData.Period, periods, ref needsCommit);
                    _uow.Import.CreateWarehouseIfNotExists(bookData.Warehouse, warehouses, ref needsCommit);
                }

                if (needsCommit)
                {
                    await _uow.CommitAsync();

                    authors = await _uow.Import.GetAllAuthorsAsync();
                    genres = await _uow.Import.GetAllGenresAsync();
                    kinds = await _uow.Import.GetAllKindsAsync();
                    periods = await _uow.Import.GetAllPeriodsAsync();
                    warehouses = await _uow.Import.GetAllWarehousesAsync();
                }

                int successCount = 0;
                int errorCount = 0;

                foreach (var bookData in importedBooksData)
                {
                    try
                    {
                        var author = _uow.Import.FindAuthor(bookData.Author, authors);
                        var genre = _uow.Import.FindGenre(bookData.Genre, genres);
                        var kind = _uow.Import.FindKind(bookData.Kind, kinds);
                        var period = _uow.Import.FindPeriod(bookData.Period, periods);
                        var warehouse = _uow.Import.FindWarehouse(bookData.Warehouse, warehouses);

                        ushort? pages = null;
                        if (!string.IsNullOrEmpty(bookData.Pages) && ushort.TryParse(bookData.Pages, out var parsedPages))
                        {
                            pages = parsedPages;
                        }

                        DateTime? dateRelease = null;
                        if (!string.IsNullOrEmpty(bookData.DateRelease) && DateTime.TryParse(bookData.DateRelease, out var parsedDate))
                        {
                            dateRelease = parsedDate;
                        }

                        var book = new Book
                        {
                            Name = bookData.Name,
                            ISBN = string.IsNullOrEmpty(bookData.ISBN) ? null : bookData.ISBN,
                            AuthorId = author?.Id,
                            GenreId = genre?.Id,
                            KindId = kind?.Id,
                            PeriodId = period?.Id,
                            Pages = pages,
                            DateRelease = dateRelease,
                            WarehouseId = warehouse?.Id,
                            Borrowable = true
                        };

                        _uow.Import.AddBook(book);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        _logger.LogError(ex, "Chyba při vytváření knihy: {BookName}", bookData.Name);
                    }
                }

                await _uow.CommitAsync();

                _logger.LogInformation("Import dokončen: {SuccessCount} knih importováno, {ErrorCount} chyb", successCount, errorCount);

                var message = $"Import dokončen! Úspěšně importováno {successCount} knih.";
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
                    var bookData = new ImportedBookData
                    {
                        Name = row.Cell(1).GetValue<string>(),
                        Author = row.Cell(2).GetValue<string>(),
                        ISBN = row.Cell(3).GetValue<string>(),
                        Genre = row.Cell(4).GetValue<string>(),
                        Kind = row.Cell(5).GetValue<string>(),
                        Period = row.Cell(6).GetValue<string>(),
                        Pages = row.Cell(7).GetValue<string>(),
                        DateRelease = row.Cell(8).GetValue<string>(),
                        Warehouse = row.Cell(9).GetValue<string>()
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

        private record ImportedBookData
        {
            public string Name { get; init; } = "";
            public string Author { get; init; } = "";
            public string ISBN { get; init; } = "";
            public string Genre { get; init; } = "";
            public string Kind { get; init; } = "";
            public string Period { get; init; } = "";
            public string Pages { get; init; } = "";
            public string DateRelease { get; init; } = "";
            public string Warehouse { get; init; } = "";
        }
    }
}
