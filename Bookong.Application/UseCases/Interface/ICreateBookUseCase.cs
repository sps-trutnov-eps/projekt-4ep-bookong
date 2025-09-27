using Bookong.Application.DTOs;
using Bookong.Domain.Entities;

namespace Bookong.Application.UseCases.Interface
{
    public interface ICreateBookUseCase
    {
        Task<Book> HandleAsync(CreateBookDto dto);
    }
}
