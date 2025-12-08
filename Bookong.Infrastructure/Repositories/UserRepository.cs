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
            _context.Users.Add(user);
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByPublicIdAsync(Guid publicId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.PublicId == publicId);
        }

        public async Task<User?> GetBySamAccountNameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.SamAccountName == username);
        }

        public Task<PagedResult<User>> GetPagedAsync(int pageNumber = 1, int pageSize = 25)
        {
            throw new NotImplementedException();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }
    }
}
