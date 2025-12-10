using System;
using System.Linq;
using System.Threading.Tasks;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Domain.Interfaces;

namespace Bookong.Application.UseCases.Queries
{
 public class CheckDuplicateQuery : ICheckDuplicateQuery
 {
 private readonly IUnitOfWork _uow;
 public CheckDuplicateQuery(IUnitOfWork uow)
 {
 _uow = uow;
 }

 public async Task<bool> AuthorExistsAsync(string firstName, string? middleName, string lastName)
 {
 var authors = await _uow.Authors.GetAllAsync();
 return authors.Any(a => string.Equals(a.FirstName?.Trim(), firstName.Trim(), StringComparison.OrdinalIgnoreCase)
 && string.Equals(a.LastName?.Trim(), lastName.Trim(), StringComparison.OrdinalIgnoreCase)
 && ((string.IsNullOrWhiteSpace(a.MiddleName) && string.IsNullOrWhiteSpace(middleName)) || string.Equals(a.MiddleName?.Trim(), middleName?.Trim(), StringComparison.OrdinalIgnoreCase)));
 }

 public async Task<bool> NameExistsAsync(string entityType, string name)
 {
 name = name?.Trim() ?? string.Empty;
 var lower = entityType?.Trim().ToLowerInvariant() ?? string.Empty;

 if (lower == "genre" || lower == "genres")
 {
 var list = await _uow.Genres.GetAllAsync();
 return list.Any(e => string.Equals(e.Name?.Trim(), name, StringComparison.OrdinalIgnoreCase));
 }

 if (lower == "kind" || lower == "kinds")
 {
 var list = await _uow.Kinds.GetAllAsync();
 return list.Any(e => string.Equals(e.Name?.Trim(), name, StringComparison.OrdinalIgnoreCase));
 }

 if (lower == "period" || lower == "periods")
 {
 var list = await _uow.Periods.GetAllAsync();
 return list.Any(e => string.Equals(e.Name?.Trim(), name, StringComparison.OrdinalIgnoreCase));
 }

 if (lower == "warehouse" || lower == "warehouses")
 {
 var list = await _uow.Warehouses.GetAllAsync();
 return list.Any(e => string.Equals(e.Name?.Trim(), name, StringComparison.OrdinalIgnoreCase));
 }

 return false;
 }
 }
}
