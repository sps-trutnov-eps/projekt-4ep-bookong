namespace Bookong.Web.Models;

public class FolderDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string CreatedBy { get; set; }
    public List<FileDto> Files { get; set; } = new();
    public List<LinkDto> Links { get; set; } = new();
    public List<StudentMaterialDto> StudentMaterials { get; set; } = new();
}

public class FileDto
{
    public string FileName { get; set; }
    public string FileType { get; set; }
}

public class LinkDto
{
    public string Name { get; set; }
    public string Url { get; set; }
}

public class StudentMaterialDto
{
    public string Title { get; set; }
    public string Description { get; set; }
}
