namespace Bookong.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IAuthorRepository Authors { get; }
        IBookLoanRepository BookLoans { get; }
        IBookRepository Books { get; }
        IBookReservationRepository BookReservations { get; }
        IGenreRepository Genres { get; }
        IKindRepository Kinds { get; }
        IMaturitaBookAssignmentRequestRepository MaturitaBookAssignmentRequests { get; }
        IMaturitaBookRepository MaturitaBooks { get; }
        IMaturitaBookSelectionRepository MaturitaBookSelections { get; }
        IPeriodRepository Periods { get; }
        IPublisherRepository Publishers { get; }
        ITeachingMaterialRepository TeachingMaterials { get; }
        IUserRepository Users { get; }
        IWarehouseRepository Warehouses { get; }

        Task CommitAsync();
    }
}
