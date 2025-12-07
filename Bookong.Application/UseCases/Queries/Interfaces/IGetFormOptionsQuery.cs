using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Queries.Interfaces
{
 public interface IGetFormOptionsQuery
 {
 Task<FormOptionsDto> ExecuteAsync();
 }
}
