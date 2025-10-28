using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookong.Infrastructure.Repositories
{
    public class UserRepository(BookongDbContext context) : IUserRepository
    {
        private readonly BookongDbContext _context = context;

        public void Add(User user)
        {
            throw new NotImplementedException();
        }

        public void Delete(User user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByPublicIdAsync(Guid publicId)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetBySamAccountNameAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<User>> GetPagedAsync(int pageNumber = 1, int pageSize = 25)
        {
            throw new NotImplementedException();
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<int?> CountAsync()
        {
            var count = await _context.Users.CountAsync();
            return count;
        }
    }
}
