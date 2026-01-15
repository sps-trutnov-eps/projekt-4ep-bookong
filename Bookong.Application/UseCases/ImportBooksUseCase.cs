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

                var authors = (await _uow.Authors.GetAllAsync()).ToList();
                var publishers = (await _uow.Publishers.GetAllAsync()).ToList();
                var genres = (await _uow.Genres.GetAllAsync()).ToList();
                var kinds = (await _uow.Kinds.GetAllAsync()).ToList();

                bool needsCommit = false;

                // Create default Genre and Kind if they don't exist
                CreateGenreIfNotExists(DefaultGenreName, genres, ref needsCommit);
                CreateKindIfNotExists(DefaultKindName, kinds, ref needsCommit);

                foreach (var bookData in importedBooksData)
                {
                    CreateAuthorIfNotExists(bookData.AuthorFirstName, bookData.AuthorLastName, authors, ref needsCommit);
                    CreatePublisherIfNotExists(bookData.Publisher, publishers, ref needsCommit);
                }

                if (needsCommit)
                {
                    await _uow.CommitAsync();

                    authors = (await _uow.Authors.GetAllAsync()).ToList();
                    publishers = (await _uow.Publishers.GetAllAsync()).ToList();
                    genres = (await _uow.Genres.GetAllAsync()).ToList();
                    kinds = (await _uow.Kinds.GetAllAsync()).ToList();
                }

                // Get default Genre and Kind for assignment
                var defaultGenre = genres.FirstOrDefault(g => g.Name == DefaultGenreName);
                var defaultKind = kinds.FirstOrDefault(k => k.Name == DefaultKindName);

                int successCount = 0;
                int errorCount = 0;

                foreach (var bookData in importedBooksData)
                {
                    try
                    {
                        var author = FindAuthor(bookData.AuthorFirstName, bookData.AuthorLastName, authors);
                        var publisher = FindPublisher(bookData.Publisher, publishers);

                        DateTime? dateRelease = null;
                        if (!string.IsNullOrEmpty(bookData.YearRelease) && int.TryParse(bookData.YearRelease, out var year))
                        {
                            dateRelease = new DateTime(year, 1, 1);
                        }

                        var book = new Book
                        {
                            Name = bookData.Name,
                            ISBN = bookData.ISBN,
                            AuthorId = author?.Id,
                            PublisherId = publisher?.Id,
                            GenreId = defaultGenre?.Id,
                            KindId = defaultKind?.Id,
                            DateRelease = dateRelease,
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
                    var name = row.Cell(2).GetValue<string>();
                    var authorLastName = row.Cell(3).GetValue<string>();
                    var authorFirstName = row.Cell(4).GetValue<string>();
                    var isbn = row.Cell(5).GetValue<string>();
                    var publisher = row.Cell(6).GetValue<string>();
                    var yearRelease = row.Cell(10).GetValue<string>();

                    var bookData = new ImportedBookData
                    {
                        Name = string.IsNullOrWhiteSpace(name) ? "Unknown" : name.Trim(),
                        AuthorLastName = string.IsNullOrWhiteSpace(authorLastName) ? "Unknown" : authorLastName.Trim(),
                        AuthorFirstName = string.IsNullOrWhiteSpace(authorFirstName) ? "" : authorFirstName.Trim(),
                        ISBN = string.IsNullOrWhiteSpace(isbn) ? null : isbn.Trim(),
                        Publisher = string.IsNullOrWhiteSpace(publisher) ? "Unknown" : publisher.Trim(),
                        YearRelease = string.IsNullOrWhiteSpace(yearRelease) ? null : yearRelease.Trim()
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

        private void CreatePublisherIfNotExists(string publisherName, List<Publisher> publishers, ref bool needsCommit)
        {
            if (!publishers.Any(p => p.Name == publisherName))
            {
                var publisher = new Publisher 
                { 
                    PublicId = Guid.NewGuid(),
                    Name = publisherName 
                };
                _uow.Publishers.Add(publisher);
                publishers.Add(publisher);
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

        private Author? FindAuthor(string firstName, string lastName, IEnumerable<Author> authors)
        {
            return authors.FirstOrDefault(a => a.FirstName == firstName && a.LastName == lastName);
        }

        private Publisher? FindPublisher(string publisherName, IEnumerable<Publisher> publishers)
        {
            return publishers.FirstOrDefault(p => p.Name == publisherName);
        }

        private record ImportedBookData
        {
            public string Name { get; init; } = "";
            public string AuthorFirstName { get; init; } = "";
            public string AuthorLastName { get; init; } = "";
            public string? ISBN { get; init; }
            public string Publisher { get; init; } = "";
            public string? YearRelease { get; init; }
        }
    }
}
