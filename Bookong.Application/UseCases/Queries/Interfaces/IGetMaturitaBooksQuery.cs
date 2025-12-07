using System.Collections.Generic;
using System.Threading.Tasks;
using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Queries.Interfaces
{
    public interface IGetMaturitaBooksQuery
    {
        Task<IEnumerable<BookDetailDto>> ExecuteAsync();
    }
}