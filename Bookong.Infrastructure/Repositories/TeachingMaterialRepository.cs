using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookong.Infrastructure.Repositories
{
    public class TeachingMaterialRepository : ITeachingMaterialRepository
    {
        private readonly BookongDbContext _context;

        public TeachingMaterialRepository(BookongDbContext context)
        {
            _context = context;
        }

        public void Add(TeachingMaterial teachingMaterial)
        {
            _context.TeachingMaterials.Add(teachingMaterial);
        }

        public void Delete(TeachingMaterial teachingMaterial)
        {
            _context.TeachingMaterials.Remove(teachingMaterial);
        }

        public async Task<IEnumerable<TeachingMaterial>> GetAllAsync()
        {
            return await _context.TeachingMaterials.ToListAsync();
        }

        public async Task<TeachingMaterial?> GetByIdAsync(int id)
        {
            return await _context.TeachingMaterials.FindAsync(id);
        }

        public async Task<TeachingMaterial?> GetByPublicIdAsync(Guid publicId)
        {
            return await _context.TeachingMaterials.FirstOrDefaultAsync(t => t.PublicId == publicId);
        }

        public Task<PagedResult<TeachingMaterial>> GetByTitleAsync(string title, int pageNumber = 1, int pageSize = 25)
        {
            throw new NotImplementedException(); // Doplníš později podle potřeby
        }

        public Task<PagedResult<TeachingMaterial>> GetPagedAsync(int pageNumber = 1, int pageSize = 25)
        {
            throw new NotImplementedException(); // Doplníš později podle potřeby
        }

        public void Update(TeachingMaterial teachingMaterial)
        {
            _context.TeachingMaterials.Update(teachingMaterial);
        }
    }
}
