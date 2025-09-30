using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure.Repositories
{
    public class MaturitaBookAssignmentRequestRepository(BookongDbContext context) : IMaturitaBookAssignmentRequestRepository
    {
        private readonly BookongDbContext _context = context;

        public void Add(MaturitaBookAssignmentRequest request)
        {
            throw new NotImplementedException();
        }

        public void Delete(MaturitaBookAssignmentRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<MaturitaBookAssignmentRequest?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MaturitaBookAssignmentRequest?> GetByPublicIdAsync(Guid publicId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<MaturitaBookAssignmentRequest>> GetPagedAsync(int pageNumber = 1, int pageSize = 25)
        {
            throw new NotImplementedException();
        }

        public void Update(MaturitaBookAssignmentRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
