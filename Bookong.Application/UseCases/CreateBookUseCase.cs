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

            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ValidationException("Title is required.", null, nameof(dto.Title));

            Author? newAuthor = null;
            Genre? newGenre = null;
            Kind? newKind = null;
            Period? newPeriod = null;
            Publisher? newPublisher = null;
            Warehouse? newWarehouse = null;

            // Create new entities if negative temp id referenced
            if (!dto.SkipAuthor && dto.AuthorId.HasValue && dto.AuthorId.Value < 0 && dto.NewAuthors != null)
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

            if (!dto.SkipGenre && dto.GenreId.HasValue && dto.GenreId.Value < 0 && dto.NewGenres != null)
            {
                var temp = dto.NewGenres.FirstOrDefault(g => g.TempId == dto.GenreId.Value);
                if (temp != null)
                {
                    newGenre = new Genre { PublicId = Guid.NewGuid(), Name = temp.Name ?? string.Empty };
                    _unitOfWork.Genres.Add(newGenre);
                }
            }

            if (!dto.SkipKind && dto.KindId.HasValue && dto.KindId.Value < 0 && dto.NewKinds != null)
            {
                var temp = dto.NewKinds.FirstOrDefault(k => k.TempId == dto.KindId.Value);
                if (temp != null)
                {
                    newKind = new Kind { PublicId = Guid.NewGuid(), Name = temp.Name ?? string.Empty };
                    _unitOfWork.Kinds.Add(newKind);
                }
            }

            if (!dto.SkipPeriod && dto.PeriodId.HasValue && dto.PeriodId.Value < 0 && dto.NewPeriods != null)
            {
                var temp = dto.NewPeriods.FirstOrDefault(p => p.TempId == dto.PeriodId.Value);
                if (temp != null)
                {
                    newPeriod = new Period { PublicId = Guid.NewGuid(), Name = temp.Name ?? string.Empty };
                    _unitOfWork.Periods.Add(newPeriod);
                }
            }

            if (!dto.SkipPublisher && dto.PublisherId.HasValue && dto.PublisherId.Value < 0 && dto.NewPublishers != null)
            {
                var temp = dto.NewPublishers.FirstOrDefault(p => p.TempId == dto.PublisherId.Value);
                if (temp != null)
                {
                    newPublisher = new Publisher { PublicId = Guid.NewGuid(), Name = temp.Name ?? string.Empty };
                    _unitOfWork.Publishers.Add(newPublisher);
                }
            }

            if (!dto.SkipWarehouse && dto.WarehouseId.HasValue && dto.WarehouseId.Value < 0 && dto.NewWarehouses != null)
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

                    newWarehouse = new Warehouse
                    {
                        PublicId = Guid.NewGuid(),
                        Name = temp.Name ?? string.Empty,
                        Address = address
                    };

                    _unitOfWork.Warehouses.Add(newWarehouse);
                }
            }

            var book = new Book
            {
                Name = dto.Title.Trim(),
                ISBN = dto.SkipISBN ? null : dto.ISBN?.Trim(),
                Pages = dto.SkipPages ? null : (dto.Pages.HasValue ? (ushort?)dto.Pages.Value : null),
                DateRelease = dto.SkipDateRelease ? null : dto.DateRelease,
                Borrowable = dto.Borrowable
            };

            if (!dto.SkipAuthor)
            {
                if (newAuthor != null) book.Author = newAuthor;
                else if (dto.AuthorId.HasValue && dto.AuthorId.Value > 0) book.AuthorId = dto.AuthorId.Value;
            }

            if (!dto.SkipGenre)
            {
                if (newGenre != null) book.Genre = newGenre;
                else if (dto.GenreId.HasValue && dto.GenreId.Value > 0) book.GenreId = dto.GenreId.Value;
            }

            if (!dto.SkipKind)
            {
                if (newKind != null) book.Kind = newKind;
                else if (dto.KindId.HasValue && dto.KindId.Value > 0) book.KindId = dto.KindId.Value;
            }

            if (!dto.SkipPeriod)
            {
                if (newPeriod != null) book.Period = newPeriod;
                else if (dto.PeriodId.HasValue && dto.PeriodId.Value > 0) book.PeriodId = dto.PeriodId.Value;
            }

            if (!dto.SkipPublisher)
            {
                if (newPublisher != null) book.Publisher = newPublisher;
                else if (dto.PublisherId.HasValue && dto.PublisherId.Value > 0) book.PublisherId = dto.PublisherId.Value;
            }

            if (!dto.SkipWarehouse)
            {
                if (newWarehouse != null) book.Warehouse = newWarehouse;
                else if (dto.WarehouseId.HasValue && dto.WarehouseId.Value > 0) book.WarehouseId = dto.WarehouseId.Value;
            }

            _unitOfWork.Books.Add(book);
            await _unitOfWork.CommitAsync();

            return book;
        }
    }
}
