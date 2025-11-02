using System.Collections.Generic;
using Bookong.Web.Models;
namespace Bookong.Web.Models;

public class FolderDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string CreatedBy { get; set; }
    public List<FileDto> Files { get; set; } = new List<FileDto>();
    public List<LinkDto> Links { get; set; } = new List<LinkDto>();
    public List<StudentMaterialDto> StudentMaterials { get; set; } = new List<StudentMaterialDto>();
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