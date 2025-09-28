using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface IImportBooksFromExcelUseCase
    {
         Task<GenericResponse> HandleAsync(Stream excelStream);
    }
}
