using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Interfaces;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;

namespace Bookong.Application.UseCases
{
    public class CreateBookUseCase : ICreateBookUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBookUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Book> ExecuteAsync(CreateBookDto dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            // Basic validation consistent with DTO attributes
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ValidationException("Title is required.", null, nameof(dto.Title));
            if (!dto.AuthorId.HasValue)
                throw new ValidationException("AuthorId is required.", null, nameof(dto.AuthorId));
            if (!dto.GenreId.HasValue)
                throw new ValidationException("GenreId is required.", null, nameof(dto.GenreId));
            if (!dto.KindId.HasValue)
                throw new ValidationException("KindId is required.", null, nameof(dto.KindId));
            if (!dto.PeriodId.HasValue)
                throw new ValidationException("PeriodId is required.", null, nameof(dto.PeriodId));
            if (!dto.Pages.HasValue || dto.Pages.Value == 0)
                throw new ValidationException("Pages must be provided and greater than zero.", null, nameof(dto.Pages));

            // Prepare variables for newly created related entities
            Author? newAuthor = null;
            Genre? newGenre = null;
            Kind? newKind = null;
            Period? newPeriod = null;
            Warehouse? newWarehouse = null;

            // Create new author if negative temp id referenced
            if (dto.AuthorId.HasValue && dto.AuthorId.Value < 0 && dto.NewAuthors != null)
            {
                var temp = dto.NewAuthors.FirstOrDefault(a => a.TempId == dto.AuthorId.Value);
                if (temp != null)
                {
                    newAuthor = new Author
                    {
                        PublicId = Guid.NewGuid(),
                        FirstName = temp.FirstName ?? string.Empty,
                        MiddleName = temp.MiddleName ?? string.Empty,
                        LastName = temp.LastName ?? string.Empty
                    };

                    _unitOfWork.Authors.Add(newAuthor);
                }
            }

            if (dto.GenreId.HasValue && dto.GenreId.Value < 0 && dto.NewGenres != null)
            {
                var temp = dto.NewGenres.FirstOrDefault(g => g.TempId == dto.GenreId.Value);
                if (temp != null)
                {
                    newGenre = new Genre { PublicId = Guid.NewGuid(), Name = temp.Name ?? string.Empty };
                    _unitOfWork.Genres.Add(newGenre);
                }
            }

            if (dto.KindId.HasValue && dto.KindId.Value < 0 && dto.NewKinds != null)
            {
                var temp = dto.NewKinds.FirstOrDefault(k => k.TempId == dto.KindId.Value);
                if (temp != null)
                {
                    newKind = new Kind { PublicId = Guid.NewGuid(), Name = temp.Name ?? string.Empty };
                    _unitOfWork.Kinds.Add(newKind);
                }
            }

            if (dto.PeriodId.HasValue && dto.PeriodId.Value < 0 && dto.NewPeriods != null)
            {
                var temp = dto.NewPeriods.FirstOrDefault(p => p.TempId == dto.PeriodId.Value);
                if (temp != null)
                {
                    newPeriod = new Period { PublicId = Guid.NewGuid(), Name = temp.Name ?? string.Empty };
                    _unitOfWork.Periods.Add(newPeriod);
                }
            }

            if (dto.WarehouseId.HasValue && dto.WarehouseId.Value < 0 && dto.NewWarehouses != null)
            {
                var temp = dto.NewWarehouses.FirstOrDefault(w => w.TempId == dto.WarehouseId.Value);
                if (temp != null)
                {
                    var address = new Address
                    {
                        PublicId = Guid.NewGuid(),
                        Street = temp.Street ?? string.Empty,
                        Number = temp.Number ?? string.Empty,
                        City = temp.City ?? string.Empty,
                        ZipCode = temp.ZipCode ?? string.Empty
                    };

                    // Warehouse requires Address navigation property
                    newWarehouse = new Warehouse
                    {
                        PublicId = Guid.NewGuid(),
                        Name = temp.Name ?? string.Empty,
                        Address = address,
                        AddressId = 0
                    };

                    _unitOfWork.Warehouses.Add(newWarehouse);
                }
            }

            // Create Book entity. For newly created related entities we set navigation property; for existing ones set the FK.
            var book = new Book
            {
                Name = dto.Title.Trim(),
                ISBN = string.IsNullOrWhiteSpace(dto.ISBN) ? null : dto.ISBN.Trim(),
                Pages = dto.Pages.Value,
                DateRelease = dto.DateRelease,
                Borrowable = true
            };

            if (newAuthor != null)
            {
                book.Author = newAuthor;
            }
            else if (dto.AuthorId.HasValue && dto.AuthorId.Value > 0)
            {
                book.AuthorId = dto.AuthorId.Value;
            }

            if (newGenre != null)
            {
                book.Genre = newGenre;
            }
            else if (dto.GenreId.HasValue && dto.GenreId.Value > 0)
            {
                book.GenreId = dto.GenreId.Value;
            }

            if (newKind != null)
            {
                book.Kind = newKind;
            }
            else if (dto.KindId.HasValue && dto.KindId.Value > 0)
            {
                book.KindId = dto.KindId.Value;
            }

            if (newPeriod != null)
            {
                book.Period = newPeriod;
            }
            else if (dto.PeriodId.HasValue && dto.PeriodId.Value > 0)
            {
                book.PeriodId = dto.PeriodId.Value;
            }

            if (newWarehouse != null)
            {
                book.Warehouse = newWarehouse;
            }
            else if (dto.WarehouseId.HasValue && dto.WarehouseId.Value > 0)
            {
                book.WarehouseId = dto.WarehouseId.Value;
            }

            _unitOfWork.Books.Add(book);

            // Commit all changes atomically
            await _unitOfWork.CommitAsync();

            return book;
        }
    }
}
