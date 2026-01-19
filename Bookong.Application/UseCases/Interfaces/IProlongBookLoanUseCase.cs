using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface IProlongBookLoanUseCase
    {
        Task<GenericResponse> ExecuteAsync(ProlongBookLoanDto dto);
    }
}
