namespace Bookong.Application.DTOs
{
    public class BookFormOptionsDto
    {
        public List<AuthorDto> Authors { get; set; } = new();
        public List<CodeListDto> Genres { get; set; } = new();
        public List<CodeListDto> Kinds { get; set; } = new();
        public List<CodeListDto> Periods { get; set; } = new();
    }

    public class AuthorDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
    }

    public class CodeListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }
}
