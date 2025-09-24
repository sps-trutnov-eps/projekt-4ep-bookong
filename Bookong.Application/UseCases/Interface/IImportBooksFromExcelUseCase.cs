using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interface
{
    public interface IImportBooksFromExcelUseCase
    {
         public Task<GenericResponse> Handle(Stream excelStream);
    }
}
