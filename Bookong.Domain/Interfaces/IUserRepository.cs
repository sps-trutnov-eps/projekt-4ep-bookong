using Bookong.Domain.Entities;

namespace Bookong.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> UserExistsByEmailAsync(string email);
        Task AddAsync(User user);
    }
}
