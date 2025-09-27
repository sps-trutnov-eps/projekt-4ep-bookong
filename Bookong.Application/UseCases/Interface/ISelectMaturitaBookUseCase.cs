using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interface
{
    public interface ISelectMaturitaBookUseCase
    {
        Task<GenericResponse> Handle(AddBookToPersonalMaturitaListDto dto);
    }
}
