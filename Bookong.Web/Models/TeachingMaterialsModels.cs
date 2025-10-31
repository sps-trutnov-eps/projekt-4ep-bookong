using Bookong.Web.Models;
namespace Bookong.Web.Models;

public class FolderDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string CreatedBy { get; set; }
    public List<FileDto> Files { get; set; } = [];
    public List<LinkDto> Links { get; set; } = [];
    public List<StudentMaterialDto> StudentMaterials { get; set; } = [];
}

public class FileDto
{
    public required string FileName { get; set; }
    public required string FileType { get; set; }
}

public class LinkDto
{
    public required string Name { get; set; }
    public required string Url { get; set; }
}

public class StudentMaterialDto
{
    public required string Title { get; set; }
    public required string Description { get; set; }
}