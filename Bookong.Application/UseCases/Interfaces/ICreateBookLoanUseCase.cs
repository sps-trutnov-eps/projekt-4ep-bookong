using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface ICreateBookLoanUseCase
    {
        Task<GenericResponse> HandleAsync(CreateBookLoanDto createBookLoanDto);
    }
}
