using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;
using Bookong.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Bookong.Infrastructure.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookongDbContext _context;
        private IDbContextTransaction? _currentTransaction;

        public IAuthorRepository Authors { get; }
        public IBookLoanRepository BookLoans { get; }
        public IBookRepository Books { get; }
        public IBookReservationRepository BookReservations { get; }
        public IGenreRepository Genres { get; }
        public IKindRepository Kinds { get; }
        public IMaturitaBookAssignmentRequestRepository MaturitaBookAssignmentRequests { get; }
        public IMaturitaBookRepository MaturitaBooks { get; }
        public IMaturitaBookSelectionRepository MaturitaBookSelections { get; }
        public IPeriodRepository Periods { get; }
        public IPublisherRepository Publishers { get; }
        public ITeachingMaterialRepository TeachingMaterials { get; }
        public IUserRepository Users { get; }
        public IWarehouseRepository Warehouses { get; }
        public ISubjectRepository Subjects { get; }
        public IBranchRepository Branches { get; }

        public UnitOfWork(BookongDbContext context)
        {
            _context = context;
            Authors = new AuthorRepository(_context);
            BookLoans = new BookLoanRepository(_context);
            Books = new BookRepository(_context);
            BookReservations = new BookReservationRepository(_context);
            Genres = new GenreRepository(_context);
            Kinds = new KindRepository(_context);
            MaturitaBookAssignmentRequests = new MaturitaBookAssignmentRequestRepository(_context);
            MaturitaBooks = new MaturitaBookRepository(_context);
            MaturitaBookSelections = new MaturitaBookSelectionRepository(_context);
            Periods = new PeriodRepository(_context);
            Publishers = new PublisherRepository(_context);
            TeachingMaterials = new TeachingMaterialRepository(_context);
            Users = new UserRepository(_context);
            Warehouses = new WarehouseRepository(_context);
            Subjects = new SubjectRepository(_context);
            Branches = new BranchRepository(_context);
        }

        public async Task CommitAsync()
        {
            _currentTransaction ??= await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.SaveChangesAsync();
                await _currentTransaction.CommitAsync();
            }
            catch (Exception)
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.RollbackAsync();
                }
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }
    }
}
