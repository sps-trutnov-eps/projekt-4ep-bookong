# GitHub Copilot Instructions for Bookong Project

## Project Architecture

Bookong follows a clean layered architecture with strict separation of concerns:

**Domain** → **Application** → **Infrastructure** + **Web**

### Layer Responsibilities

#### Domain (Bookong.Domain)
- Entities, repository interfaces, IUnitOfWork
- **No implementation logic, no dependencies**
- Don't modify entities unless explicitly required

#### Application (Bookong.Application)
- UseCases (business logic), DTOs, Service interfaces
- Use `IUnitOfWork` for all data access
- Return `GenericResponse` from UseCases
- UseCases in `UseCases/`, interfaces in `UseCases/Interfaces/`

#### Infrastructure (Bookong.Infrastructure)
- Repository implementations, UnitOfWork, DbContext, migrations
- Use `.AsNoTracking()` for read-only queries
- Use `.Include()` for navigation properties
- **No business logic**

#### Web (Bookong.Web)
- Blazor components and pages
- Inject UseCases/Queries, **NEVER IUnitOfWork directly**
- Handle GenericResponse for user feedback

## Key Patterns

### Unit of Work
```csharp
public class SomeUseCase : ISomeUseCase
{
    private readonly IBookRepository _books;

    public SomeUseCase(IUnitOfWork unitOfWork)
    {
        _books = unitOfWork.Books;
    }

    public async Task<GenericResponse> ExecuteAsync(SomeDto dto)
    {
        var book = await _books.GetByIdAsync(dto.BookId);
        // ... business logic
        await unitOfWork.CommitAsync(); // Always commit!
        return GenericResponse.SuccessResponse("Success", book);
    }
}
```

### Blazor Components
```razor
@inject ICreateBookUseCase CreateBook

@code {
    private async Task HandleSubmit()
    {
        var response = await CreateBook.ExecuteAsync(dto);
        if (response.Success) { /* success */ }
        else { /* show error */ }
    }
}
```

## Naming Conventions

- **PascalCase**: Classes, Methods, Properties, Constants
- **camelCase**: Variables, Parameters
- **_camelCase**: Private fields
- **IPascalCase**: Interfaces
- **PascalCaseAsync**: Async methods
- **PascalCaseDto**: DTOs

## Adding New Features

1. **Domain**: Add entity + repository interface → Update IUnitOfWork
2. **Infrastructure**: Implement repository → Add to UnitOfWork → Create migration
3. **Application**: Create DTO + UseCase interface + UseCase implementation
4. **Web**: Register in Program.cs → Create Blazor component → Inject UseCase

## Critical Rules

❌ **NEVER**:
- Inject IUnitOfWork into Blazor components
- Put business logic in repositories or Blazor components
- Forget `await unitOfWork.CommitAsync()` after data changes
- Use synchronous database operations
- Modify Domain entities without requirement

✅ **ALWAYS**:
- Use IUnitOfWork in UseCases for data access
- Use `.AsNoTracking()` for read-only queries
- Register UseCases in Program.cs
- Handle exceptions in UseCases with GenericResponse
- Use async/await throughout

## Tech Stack
.NET 9, Blazor Server, EF Core, SQL Server, Bootstrap 5

---

## Code Review Guidelines

### Architecture Checklist
- [ ] Kód ve správné vrstvě (Domain/Application/Infrastructure/Web)?
- [ ] Business logika pouze v UseCases?
- [ ] IUnitOfWork použit jen v UseCases, ne v Blazor komponentách?
- [ ] CommitAsync() volán po změnách dat?
- [ ] AsNoTracking() použito pro read-only dotazy?
- [ ] UseCases zaregistrovány v Program.cs?
- [ ] Async/await použito konzistentně?
- [ ] Naming conventions dodrženy (PascalCase, camelCase, _camelCase)?
- [ ] DTOs mají validaci s českými chybovými zprávami?
- [ ] Navigační vlastnosti includovány kde je potřeba?

### Review Comment Style (Czech)
Write reviews in Czech with constructive, educational tone:
- ✅ "Tady by bylo lepší použít IUnitOfWork. Podívej se do sekce Unit of Work výše."
- ✅ "Skvělé použití GenericResponse!"
- ✅ "CommitAsync je potřeba pro uložení změn do databáze."
- ❌ "Tohle je špatně."
