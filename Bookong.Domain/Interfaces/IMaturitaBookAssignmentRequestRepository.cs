using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;

namespace Bookong.Domain.Interfaces
{
    public interface IMaturitaBookAssignmentRequestRepository
    {
        Task<MaturitaBookAssignmentRequest?> GetByIdAsync(int id);
        Task<MaturitaBookAssignmentRequest?> GetByPublicIdAsync(Guid publicId);
        Task<PagedResult<MaturitaBookAssignmentRequest>> GetPagedAsync(int pageNumber = 1, int pageSize = 25);
        void Add(MaturitaBookAssignmentRequest request);
        void Update(MaturitaBookAssignmentRequest request);
        void Delete(MaturitaBookAssignmentRequest request);
    }
}
