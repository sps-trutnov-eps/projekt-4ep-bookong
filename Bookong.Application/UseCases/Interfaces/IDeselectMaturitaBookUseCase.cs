using Bookong.Application.DTOs;
using System.Threading.Tasks;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface IDeselectMaturitaBookUseCase
    {
Task<GenericResponse> Handle(RemoveBookFromPersonalMaturitaListDto dto);
    }
}
