using System;
using System.Threading.Tasks;

namespace Bookong.Application.UseCases.Commands.Interfaces
{
    public interface IAddBookToMaturitaSelectionCommand
    {
        Task ExecuteAsync(Guid bookPublicId);
    }
}