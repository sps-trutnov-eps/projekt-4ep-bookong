using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;

namespace Bookong.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByPublicIdAsync(Guid publicId);
        Task<User?> GetBySamAccountNameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<PagedResult<User>> GetPagedAsync(int pageNumber = 1, int pageSize = 25);
        Task<IEnumerable<User>> GetAllAsync();
        void Add(User user);
        void Update(User user);
        void Delete(User user);

        Task<int?>  CountAsync();
    }
}
