using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure.Repositories
{
    public class AuthorRepository(BookongDbContext context) : IAuthorRepository
    {
        private readonly BookongDbContext _context = context;

        void IAuthorRepository.Add(Author author)
        {
            throw new NotImplementedException();
        }

        void IAuthorRepository.Delete(Author author)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Author>> IAuthorRepository.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<Author?> IAuthorRepository.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<PagedResult<Author>> IAuthorRepository.GetByNameAsync(string name, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        Task<PagedResult<Author>> IAuthorRepository.GetPagedAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        void IAuthorRepository.Update(Author author)
        {
            throw new NotImplementedException();
        }
    }
}
