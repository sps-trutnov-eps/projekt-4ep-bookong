using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Domain.Queries;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookong.Infrastructure.Repositories
{
    public class MaturitaBookSelectionRepository(BookongDbContext context) : IMaturitaBookSelectionRepository
    {
        private readonly BookongDbContext _context = context;

        public async Task<IEnumerable<MaturitaBookSelection>> GetAllAsync()
        {
            return await _context.MaturitaBookSelections
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<MaturitaBookSelection?> GetByIdAsync(int id)
        {
            return await _context.MaturitaBookSelections
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedResult<MaturitaBookSelection>> GetByUserAsync(User user, int pageNumber = 1, int pageSize = 25)
        {
            var query = _context.MaturitaBookSelections
                .Include(m => m.Book).ThenInclude(b => b.Author)
                .Include(m => m.Book).ThenInclude(b => b.Genre)
                .Include(m => m.Book).ThenInclude(b => b.Kind)
                .Where(m => m.UserId == user.Id)
                .AsNoTracking();

            var totalCount = await query.CountAsync();

            if (totalCount == 0)
                return new PagedResult<MaturitaBookSelection>(Enumerable.Empty<MaturitaBookSelection>(), 0, pageNumber, pageSize);

            var items = await query
                .OrderBy(m => m.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<MaturitaBookSelection>(items, totalCount, pageNumber, pageSize);
        }

        public Task<PagedResult<MaturitaBookSelection>> GetByUserFilteredAsync(User user, BookSearchCriteria criteria, int pageNumber = 1, int pageSize = 25)
        {
            // Pokud budou potřeba konkrétní filtry, doplnit podle BookSearchCriteria.
            return GetByUserAsync(user, pageNumber, pageSize);
        }

        // Přidáno: implementace Add a Delete tak, aby odpovídaly rozhraní
        public void Add(MaturitaBookSelection maturitaBookSelection)
        {
            if (maturitaBookSelection == null) throw new ArgumentNullException(nameof(maturitaBookSelection));
            _context.MaturitaBookSelections.Add(maturitaBookSelection);
        }

        public void Delete(MaturitaBookSelection maturitaBookSelection)
        {
            if (maturitaBookSelection == null) throw new ArgumentNullException(nameof(maturitaBookSelection));
            _context.MaturitaBookSelections.Remove(maturitaBookSelection);
        }

        // Volitelně: vrací výběr uživatele s načtenými souvisejícími entitami
        public async Task<IEnumerable<MaturitaBookSelection>> GetByUserWithDetailsAsync(int userId)
        {
            return await _context.MaturitaBookSelections
                .Include(m => m.Book).ThenInclude(b => b.Author)
                .Include(m => m.Book).ThenInclude(b => b.Genre)
                .Include(m => m.Book).ThenInclude(b => b.Kind)
                .Where(m => m.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
