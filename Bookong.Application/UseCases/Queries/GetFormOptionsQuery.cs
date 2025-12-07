using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Domain.Interfaces;

namespace Bookong.Application.UseCases.Queries
{
 public class GetFormOptionsQuery : IGetFormOptionsQuery
 {
 private readonly IUnitOfWork _uow;
 public GetFormOptionsQuery(IUnitOfWork uow)
 {
 _uow = uow;
 }

 public async Task<FormOptionsDto> ExecuteAsync()
 {
 var authors = (await _uow.Authors.GetAllAsync()).Select(a => new SelectItemDto { Id = a.Id, Name = $"{a.FirstName} {a.LastName}" }).ToList();
 var genres = (await _uow.Genres.GetAllAsync()).Select(g => new SelectItemDto { Id = g.Id, Name = g.Name }).ToList();
 var kinds = (await _uow.Kinds.GetAllAsync()).Select(k => new SelectItemDto { Id = k.Id, Name = k.Name }).ToList();
 var periods = (await _uow.Periods.GetAllAsync()).Select(p => new SelectItemDto { Id = p.Id, Name = p.Name }).ToList();
 var warehouses = (await _uow.Warehouses.GetAllAsync()).Select(w => new SelectItemDto { Id = w.Id, Name = w.Name }).ToList();

 return new FormOptionsDto
 {
 Authors = authors,
 Genres = genres,
 Kinds = kinds,
 Periods = periods,
 Warehouses = warehouses
 };
 }
 }
}
