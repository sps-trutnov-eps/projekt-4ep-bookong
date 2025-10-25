using System;
using System.Threading.Tasks;

namespace Bookong.Application.UseCases.Commands.Interfaces
{
 public interface IRemoveBookFromMaturitaSelectionCommand
 {
 Task ExecuteAsync(Guid bookPublicId);
 }
}
