using System;
using System.Threading.Tasks;

namespace Bookong.Application.Services.Interfaces
{
    public interface ICurrentUserService
    {
        Task<Guid?> GetCurrentUserPublicIdAsync();
    }
}
