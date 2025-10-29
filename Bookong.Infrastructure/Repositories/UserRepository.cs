using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            return await _context.Users
                .OrderBy(u => u.Id)
                .ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByPublicIdAsync(Guid publicId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.PublicId == publicId);
        }

        public async Task<User?> GetBySamAccountNameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.SamAccountName == username);
        }

        public async Task<PagedResult<User>> GetPagedAsync(int pageNumber = 1, int pageSize = 25)
        {
            var query = _context.Users.AsQueryable();

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(u => u.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<User>(items, totalCount, pageNumber, pageSize);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }
    }
}
