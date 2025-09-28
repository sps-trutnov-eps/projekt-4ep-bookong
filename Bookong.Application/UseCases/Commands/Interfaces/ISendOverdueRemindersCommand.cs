namespace Bookong.Application.UseCases.Commands.Interfaces
{
    public interface ISendOverdueRemindersCommand
    {
        Task HandleAsync();
    }
}
