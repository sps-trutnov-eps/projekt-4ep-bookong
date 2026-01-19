using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Domain.Interfaces;

namespace Bookong.Application.UseCases.Queries
{
    public class GetBookFormOptionsQuery : IGetBookFormOptionsQuery
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBookFormOptionsQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BookFormOptionsDto> ExecuteAsync()
        {
            var authors = await _unitOfWork.Authors.GetAllAsync();
            var genres = await _unitOfWork.Genres.GetAllAsync();
            var kinds = await _unitOfWork.Kinds.GetAllAsync();
            var periods = await _unitOfWork.Periods.GetAllAsync();

            return new BookFormOptionsDto
            {
                Authors = authors.Select(a => new AuthorDto 
                { 
                    Id = a.Id, 
                    FullName = $"{a.FirstName} {a.LastName}".Trim() 
                }).OrderBy(a => a.FullName).ToList(),

                Genres = genres.Select(g => new CodeListDto 
                { 
                    Id = g.Id, 
                    Name = g.Name 
                }).OrderBy(g => g.Name).ToList(),

                Kinds = kinds.Select(k => new CodeListDto 
                { 
                    Id = k.Id, 
                    Name = k.Name 
                }).OrderBy(k => k.Name).ToList(),

                Periods = periods.Select(p => new CodeListDto 
                { 
                    Id = p.Id, 
                    Name = p.Name 
                }).OrderBy(p => p.Name).ToList()
            };
        }
    }
}
