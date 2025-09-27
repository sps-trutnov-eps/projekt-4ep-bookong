using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interface
{
    public interface IImportBooksFromExcelUseCase
    {
         Task<GenericResponse> HandleAsync(Stream excelStream);
    }
}
