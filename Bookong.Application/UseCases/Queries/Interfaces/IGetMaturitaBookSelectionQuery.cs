using System.Collections.Generic;
using System.Threading.Tasks;
using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Queries.Interfaces
{
    public interface IGetMaturitaBookSelectionQuery
    {
        Task<IEnumerable<BookListItemDto>> ExecuteAsync();
    }
}