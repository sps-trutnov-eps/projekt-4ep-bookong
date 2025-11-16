using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Interfaces;
using Bookong.Domain.Entities;

namespace Bookong.Application.UseCases
{
    /// <summary>
    /// Simple in-memory implementation of <see cref="ICreateBookUseCase"/>.
    /// Useful for tests, demos or until you wire up persistence (EF/Core repository).
    /// </summary>
    public sealed class InMemoryCreateBookUseCase : ICreateBookUseCase
    {
        private static int _nextId = 0;
        private readonly ConcurrentBag<Book> _store;

        public InMemoryCreateBookUseCase(ConcurrentBag<Book>? store = null)
        {
            _store = store ?? new ConcurrentBag<Book>();
        }

        public Task<Book> ExecuteAsync(CreateBookDto dto)
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

            var book = new Book
            {
                Id = Interlocked.Increment(ref _nextId),
                PublicId = Guid.NewGuid(),
                Name = dto.Title!.Trim(),
                ISBN = string.IsNullOrWhiteSpace(dto.ISBN) ? null : dto.ISBN!.Trim(),
                AuthorId = dto.AuthorId!.Value,
                // Minimal nav props so object initializer satisfies 'required' members.
                Author = new Author
                {
                    Id = dto.AuthorId.Value,
                    PublicId = Guid.NewGuid(),
                    FirstName = string.Empty,
                    MiddleName = string.Empty,
                    LastName = string.Empty
                },
                GenreId = dto.GenreId!.Value,
                Genre = new Genre { Id = dto.GenreId.Value, PublicId = Guid.NewGuid(), Name = string.Empty },
                KindId = dto.KindId!.Value,
                Kind = new Kind { Id = dto.KindId.Value, PublicId = Guid.NewGuid(), Name = string.Empty },
                PeriodId = dto.PeriodId!.Value,
                Period = new Period { Id = dto.PeriodId.Value, PublicId = Guid.NewGuid(), Name = string.Empty },
                Pages = dto.Pages!.Value,
                DateRelease = dto.DateRelease,
                WarehouseId = dto.WarehouseId,
                Warehouse = dto.WarehouseId.HasValue
    ? new Warehouse
    {
        Id = dto.WarehouseId.Value,
        PublicId = Guid.NewGuid(),
        Name = string.Empty,
        AddressId = 0,
        Address = new Address
        {
            Id = 0,
            PublicId = Guid.NewGuid(),
            Street = string.Empty,
            Number = string.Empty,
            City = string.Empty,
            ZipCode = string.Empty
        }
    }
    : null,
                Borrowable = true
            };

            _store.Add(book);

            return Task.FromResult(book);
        }
    }
}

namespace Bookong.Application
{
    public class ICreateBookUseCase
    {
    }
}
