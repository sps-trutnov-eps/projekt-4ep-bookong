namespace Bookong.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IAuthorRepository Authors { get; }
        public IBookLoanRepository BookLoans { get; }
        public IBookRepository Books { get; }
        public IBookReservationRepository BookReservations { get; }
        public IGenreRepository Genres { get; }
        public IImportRepository Import { get; }
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

        Task CommitAsync();
    }
}
