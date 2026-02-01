using Bookong.Application.DTOs;
using Bookong.Domain.Entities;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface ICreateBookUseCase
    {
        Task<Book> ExecuteAsync(CreateBookDto dto);
    }
}
