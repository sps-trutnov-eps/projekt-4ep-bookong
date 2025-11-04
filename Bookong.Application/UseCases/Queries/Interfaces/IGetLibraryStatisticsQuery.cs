using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Queries.Interfaces
{
    public interface IGetLibraryStatisticsQuery
    {
        Task<LibraryStatisticsDto> ExecuteAsync();
    }
}
