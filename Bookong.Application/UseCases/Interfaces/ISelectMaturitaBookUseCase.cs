using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface ISelectMaturitaBookUseCase
    {
        Task<GenericResponse> Handle(AddBookToPersonalMaturitaListDto dto);
    }
}
