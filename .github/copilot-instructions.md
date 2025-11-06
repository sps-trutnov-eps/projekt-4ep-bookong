# GitHub Copilot Instructions for Bookong Project

> **This is an educational project.** Prioritize clean, understandable code that demonstrates best practices over clever shortcuts. Write code that teaches.

## Project Architecture

Bookong follows a clean layered architecture with strict separation of concerns:

**Domain** → **Application** → **Infrastructure** + **Web**

### Layer Responsibilities

#### Domain (Bookong.Domain)
- Entities, repository interfaces, IUnitOfWork
- **No implementation logic, no dependencies**
- **DO NOT modify entities unless explicitly required by the feature**
- This layer should remain stable - changes here affect the entire application

#### Application (Bookong.Application)
- UseCases (business logic), DTOs, Service interfaces, Queries, Commands
- Use `IUnitOfWork` for all data access
- Return `GenericResponse` from UseCases
- UseCases in `UseCases/`, Queries in `UseCases/Queries/`, Commands in `UseCases/Commands/`

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

### Query vs UseCase vs Command

#### Query (Read Operations)
**Purpose:** Retrieve data without modifying anything

**Characteristics:**
- 📖 **Read-only** operations
- ❌ **No database changes**
- ✅ Returns data (DTOs, lists)
- 👤 **Triggered by:** User viewing data
- 🔍 Located in `Bookong.Application.UseCases.Queries`

**Example:**
```csharp
public interface IGetAvailableBooksQuery
{
    Task<IEnumerable<BookListItemDto>> ExecuteAsync();
}
```

**When to use:**
- Fetching lists (books, users, etc.)
- Getting single entities by ID
- Dashboard statistics
- Filtering/searching data

#### UseCase (Write Operations - User Initiated)
**Purpose:** Perform actions that **change state** based on **user input**

**Characteristics:**
- ✏️ **Modifies data** (Create, Update, Delete)
- 👤 **Triggered by:** User action (button click, form submit)
- 🧠 **Contains business logic** and validation
- 🔄 Uses `IUnitOfWork` for transactions
- ✅ Returns `GenericResponse` (success/failure with messages)
- 📁 Located in `Bookong.Application.UseCases`

**Example:**
```csharp
public interface ISelectMaturitaBookUseCase
{
    Task<GenericResponse> Handle(AddBookToPersonalMaturitaListDto dto);
}
```

**When to use:**
- Adding a book to maturita list (user clicks button)
- Removing a book from maturita list (user clicks button)
- Creating a new user (user submits form)
- Updating book information (user edits data)
- Any operation with business rules triggered by user

#### Command (Background Operations - Application Initiated)
**Purpose:** Perform actions that the **application triggers automatically**

**Characteristics:**
- 🤖 **Triggered by:** Application itself (scheduled tasks, background jobs, system events)
- ❌ **Not triggered by user** directly
- ✅ No user input required
- ✅ May return `void` or simple success/failure
- 📁 Located in `Bookong.Application.UseCases.Commands`

**Example:**
```csharp
public interface ISendOverdueBookRemindersCommand
{
    Task ExecuteAsync();
}
```

**When to use:**
- Automatically send overdue book reminders (scheduled daily)
- Clean up expired maturita selections (scheduled task)
- Generate daily library statistics report (background job)
- Send notification emails (triggered by system event)

**Important:** Don't confuse with CQRS "Commands". In this project:
- **UseCase** = User clicks button → app responds
- **Command** = App runs automatically → no user interaction

### Unit of Work
```csharp
public class SomeUseCase : ISomeUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBookRepository _books;

    public SomeUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _books = unitOfWork.Books;
    }

    public async Task<GenericResponse> Handle(SomeDto dto)
    {
        var book = await _books.GetByIdAsync(dto.BookId);
        // ... business logic
        await _unitOfWork.CommitAsync(); // Always commit!
        return GenericResponse.SuccessResponse("Success", book);
    }
}
```

### Blazor Components
```razor
@inject ISelectMaturitaBookUseCase SelectMaturitaBook

@code {
    private async Task HandleSubmit()
    {
        var response = await SelectMaturitaBook.Handle(dto);
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

1. **Domain**: Add entity + repository interface → Update IUnitOfWork (only if truly needed!)
2. **Infrastructure**: Implement repository → Add to UnitOfWork → Create migration
3. **Application**: Create DTO + UseCase/Query/Command interface + implementation
4. **Web**: Register in Program.cs → Create Blazor component → Inject UseCase/Query
5. **Tests**: Consider adding unit tests for UseCases (see Testing Guidelines below)

## Critical Rules

❌ **NEVER**:
- Inject IUnitOfWork into Blazor components
- Put business logic in repositories or Blazor components
- Forget `await unitOfWork.CommitAsync()` after data changes
- Use synchronous database operations
- Put user-initiated operations in Commands folder
- Put application-initiated operations in UseCases folder
- Modify Domain entities without explicit requirement
- Leave debugging code, commented-out code, or verbose explanatory comments before committing, unless justified

✅ **ALWAYS**:
- Use IUnitOfWork in UseCases for data access
- Use `.AsNoTracking()` for read-only queries
- Register UseCases/Queries/Commands in Program.cs
- Handle exceptions in UseCases with GenericResponse
- Use async/await throughout
- Put user-initiated operations (button clicks) in UseCases
- Put application-initiated operations (scheduled tasks) in Commands
- Use Queries for read-only operations
- Keep code clean and lean before committing
- Write self-documenting code over explanatory comments

## Code Cleanliness

Before committing, remove:
- ❌ Debugging statements (`Console.WriteLine`, temporary logs)
- ❌ Commented-out code from troubleshooting
- ❌ Verbose comments explaining obvious things
- ❌ Unused using statements
- ❌ Temporary variables from debugging

Keep:
- ✅ Comments explaining "why", not "what"
- ✅ XML documentation for public APIs
- ✅ Clean, self-documenting code

## Tech Stack
.NET 9, Blazor Server, EF Core, SQL Server, Bootstrap 5, xUnit

---

## Blazor & Frontend Guidelines

### Styling & CSS
- **Use Bootstrap 5 classes** for all styling whenever possible
- **Minimize custom CSS** - only create custom styles when Bootstrap doesn't provide a solution
- Use Bootstrap utility classes: `mb-3`, `mt-4`, `d-flex`, `justify-content-center`, etc.
- For custom styles, place in component-specific `<style>` blocks with scoped CSS

### Component Structure
- **Create reusable components** for repeated UI patterns
- Place reusable components in `Components/Shared/` or `Components/`
- Place page-specific components in `Components/Pages/`
- Use `[Parameter]` for component inputs
- Use `EventCallback<T>` for component outputs

### Reusable Component Example
```razor
@* Components/Shared/AlertMessage.razor *@
<div class="alert alert-@Type alert-dismissible fade show" role="alert">
    @Message
    <button type="button" class="btn-close" @onclick="OnClose"></button>
</div>

@code {
    [Parameter] public string Message { get; set; } = "";
    [Parameter] public string Type { get; set; } = "info"; // info, success, warning, danger
    [Parameter] public EventCallback OnClose { get; set; }
}
```

### Bootstrap Best Practices
```razor
@* ✅ GOOD - Using Bootstrap classes *@
<div class="container mt-4">
    <div class="row">
        <div class="col-md-6">
            <button class="btn btn-primary">Submit</button>
        </div>
    </div>
</div>

@* ❌ BAD - Unnecessary custom CSS *@
<div style="margin-top: 20px; width: 50%;">
    <button style="background: blue; color: white;">Submit</button>
</div>
```

### Forms & Validation
```razor
<EditForm Model="@dto" OnValidSubmit="HandleSubmit">
    <DataAnnotationsValidator />
    <ValidationSummary class="alert alert-danger" />
    
    <div class="mb-3">
        <label class="form-label">Title</label>
        <InputText @bind-Value="dto.Title" class="form-control" />
        <ValidationMessage For="@(() => dto.Title)" class="text-danger" />
    </div>
    
    <button type="submit" class="btn btn-primary">Save</button>
</EditForm>
```

### Frontend Rules

✅ **DO**:
- Use Bootstrap components (buttons, cards, forms, modals, alerts)
- Create reusable components for common patterns (data tables, forms, alerts)
- Use Bootstrap grid system for layouts
- Use Bootstrap utility classes for spacing and positioning
- Keep components small and focused

❌ **DON'T**:
- Write custom CSS when Bootstrap provides the solution
- Inline styles except for dynamic values
- Duplicate UI patterns - create a component instead
- Mix presentation logic with business logic in components
- Use `!important` in CSS

---

## Testing Guidelines

> Writing tests is encouraged for improving code quality, but it is not a strict requirement. These guidelines are provided to ensure consistency for when tests are written.

### Test Framework
- **xUnit** for all tests
- Use **Arrange-Act-Assert** pattern

### Test Doubles Strategy
For this educational project, **prefer manual fakes over mocking libraries** to help understand how testing works.

**Option 1: Manual Fakes** (Recommended for learning)
```csharp
// Create simple fake implementations
public class FakeBookRepository : IBookRepository
{
    private readonly List<Book> _books = new();
    public List<Book> AddedBooks => _books;
    
    public void Add(Book book) => _books.Add(book);
    
    public Task<Book?> GetByIdAsync(int id) 
        => Task.FromResult(_books.FirstOrDefault(b => b.Id == id));

    public Task<IEnumerable<Book>> GetAllAsync() 
        => Task.FromResult<IEnumerable<Book>>(_books);
}

public class FakeUnitOfWork : IUnitOfWork
{
    public IBookRepository Books { get; }
    public bool CommitCalled { get; private set; }
    
    public FakeUnitOfWork(IBookRepository books)
    {
        Books = books;
    }
    
    public Task CommitAsync()
    {
        CommitCalled = true;
        return Task.CompletedTask;
    }
}
```

**Option 2: NSubstitute** (Optional, if boilerplate becomes too much)
```csharp
// Install: dotnet add package NSubstitute
var mockBooks = Substitute.For<IBookRepository>();
var mockUnitOfWork = Substitute.For<IUnitOfWork>();
mockUnitOfWork.Books.Returns(mockBooks);
```

### What to Test

**Priority 1 - UseCases** (business logic):
- ✅ Happy path scenarios
- ✅ Validation failures
- ✅ Exception handling
- ✅ GenericResponse success/failure

**Priority 2 - Domain logic**:
- ✅ Entity validation
- ✅ Domain-specific rules

**Lower priority**:
- Repositories (integration tests with real DB)
- Blazor components (UI tests)

### Test Naming Convention
```csharp
// Pattern: MethodName_Scenario_ExpectedResult
[Fact]
public async Task ExecuteAsync_ValidBook_ReturnsSuccess()

[Fact]
public async Task ExecuteAsync_BookNotFound_ReturnsFailure()
```

### Testing UseCases Example
```csharp
public class CreateBookUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ValidDto_CreatesBookAndReturnsSuccess()
    {
    // Arrange
    var fakeBooks = new FakeBookRepository();
    var fakeUnitOfWork = new FakeUnitOfWork(fakeBooks);
    var useCase = new CreateBookUseCase(fakeUnitOfWork);
    var dto = new CreateBookDto { Title = "Test Book", AuthorId = 1 };

    // Act
    var result = await useCase.ExecuteAsync(dto);

    // Assert
    Assert.True(result.Success);
    Assert.Single(fakeBooks.AddedBooks);
    Assert.True(fakeUnitOfWork.CommitCalled);
    }
    
    [Fact]
    public async Task ExecuteAsync_InvalidDto_ReturnsFailure()
    {
        // Arrange
        var fakeBooks = new FakeBookRepository();
        var fakeUnitOfWork = new FakeUnitOfWork(fakeBooks);
        var useCase = new CreateBookUseCase(fakeUnitOfWork);
        var dto = new CreateBookDto { Title = null }; // Invalid

        // Act
        var result = await useCase.ExecuteAsync(dto);

        // Assert
        Assert.False(result.Success);
        Assert.NotEmpty(result.Message);
    }
}
```

### Testing Rules

✅ **DO**:
- Create fake implementations of dependencies
- Test one scenario per test method
- Use descriptive test names
- Verify both return values and side effects
- Test async methods with `async Task`

❌ **DON'T**:
- Test framework code (EF Core, ASP.NET)
- Use real database in unit tests
- Test multiple scenarios in one test
- Skip testing edge cases and failures

### Test Organization
```
Bookong.Tests/
  ├── UseCases/
  │   ├── CreateBookUseCaseTests.cs
  │   └── UpdateBookUseCaseTests.cs
  ├── Domain/
  │   └── BookEntityTests.cs
  └── Fakes/
      ├── FakeBookRepository.cs
      ├── FakeUnitOfWork.cs
      └── TestDataBuilder.cs
```

---

## Code Review Guidelines

### Architecture Checklist
- [ ] Kód ve správné vrstvě (Domain/Application/Infrastructure/Web)?
- [ ] Business logika pouze v UseCases?
- [ ] IUnitOfWork použit jen v UseCases, ne v Blazor komponentách?
- [ ] CommitAsync() volán po změnách dat?
- [ ] AsNoTracking() použito pro read-only dotazy?
- [ ] UseCases/Queries/Commands zaregistrovány v Program.cs?
- [ ] Async/await použito konzistentně?
- [ ] Naming conventions dodrženy (PascalCase, camelCase, _camelCase)?
- [ ] DTOs mají validaci s českými chybovými zprávami?
- [ ] Navigační vlastnosti includovány kde je potřeba?
- [ ] UseCase pro user-initiated operace, Command pro application-initiated?
- [ ] Query použito pro read-only operace?
- [ ] (Pokud byly přidány) Mají nové UseCases unit testy?
- [ ] Kód je čistý - bez debug kódu, zakomentovaného kódu, zbytečných komentářů?
- [ ] Domain entity změněny jen pokud to feature vyžaduje?
- [ ] UI používá Bootstrap místo custom CSS kde je to možné?
- [ ] Opakující se UI vzory vytvořeny jako znovupoužitelné komponenty?

### Review Comment Style (Czech)
Write reviews in Czech with constructive, educational tone:
- ✅ "Tady by bylo lepší použít IUnitOfWork. Podívej se do sekce Unit of Work výše."
- ✅ "Skvělé použití GenericResponse!"
- ✅ "CommitAsync je potřeba pro uložení změn do databáze."
- ✅ "Tohle by mělo být UseCase (user-initiated), ne Command (application-initiated)."
- ✅ "Pro read-only operace použij Query místo UseCase."
- ✅ "Přidej prosím test pro tento UseCase - viz Testing Guidelines."
- ✅ "Tohle vypadá jako debug kód - prosím odstraň před mergem."
- ✅ "Místo custom CSS použij Bootstrap třídu `mb-3` pro margin-bottom."
- ✅ "Tenhle vzor se opakuje - zvaž z toho znovupoužitelnou komponentu."
- ❌ "Tohle je špatně."
