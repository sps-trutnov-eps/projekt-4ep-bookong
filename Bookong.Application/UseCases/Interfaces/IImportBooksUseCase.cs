using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface IImportBooksUseCase
    {
        Task<GenericResponse> ImportFromExcelAsync(Stream fileStream, string fileName);
    }
}
