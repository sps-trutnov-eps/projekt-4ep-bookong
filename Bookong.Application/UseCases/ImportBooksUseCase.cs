using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Interfaces;
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
                var importedBooks = new List<string>();
                
                using var memoryStream = new MemoryStream();
                await fileStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                
                using (var workbook = new XLWorkbook(memoryStream))
                {
                    var worksheet = workbook.Worksheet(1);
                    var rows = worksheet.RangeUsed().RowsUsed().Skip(1);

                    _logger.LogInformation("=== ZAČÁTEK IMPORTU EXCEL SOUBORU ===");
                    _logger.LogInformation($"Soubor: {fileName}");
                    _logger.LogInformation($"Název listu: {worksheet.Name}");

                    int rowNumber = 2;
                    foreach (var row in rows)
                    {
                        try
                        {
                            var name = row.Cell(1).GetValue<string>();
                            var author = row.Cell(2).GetValue<string>();
                            var isbn = row.Cell(3).GetValue<string>();
                            var genre = row.Cell(4).GetValue<string>();
                            var kind = row.Cell(5).GetValue<string>();
                            var period = row.Cell(6).GetValue<string>();
                            var pages = row.Cell(7).GetValue<string>();
                            var dateRelease = row.Cell(8).GetValue<string>();
                            var warehouse = row.Cell(9).GetValue<string>();

                            var bookInfo = $"Řádek {rowNumber}: " +
                                         $"Název='{name}', " +
                                         $"Autor='{author}', " +
                                         $"ISBN='{isbn}', " +
                                         $"Žánr='{genre}', " +
                                         $"Druh='{kind}', " +
                                         $"Období='{period}', " +
                                         $"Stran='{pages}', " +
                                         $"Datum='{dateRelease}', " +
                                         $"Sklad='{warehouse}'";

                            _logger.LogInformation(bookInfo);
                            importedBooks.Add(bookInfo);
                            rowNumber++;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning($"Řádek {rowNumber} - chyba při čtení: {ex.Message}");
                            rowNumber++;
                            continue;
                        }
                    }
                }

                _logger.LogInformation("=== KONEC IMPORTU ===");
                _logger.LogInformation($"Celkem načteno řádků: {importedBooks.Count}");

                return GenericResponse.SuccessResponse(
                    $"Úspěšně načteno {importedBooks.Count} knih. Zkontroluj Output window (Debug) pro detaily.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Chyba při čtení Excel souboru");
                return GenericResponse.FailureResponse($"Chyba při zpracování souboru: {ex.Message}");
            }
        }
    }
}
