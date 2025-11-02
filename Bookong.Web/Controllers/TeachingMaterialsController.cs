using Microsoft.AspNetCore.Mvc;
using Bookong.Web.Models;

namespace Bookong.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeachingMaterialsController : ControllerBase
    {
        [HttpGet("folders")]
        public IActionResult GetFolders()
        {
            var folders = new List<FolderDto>
            {
                new FolderDto
                {
                    Name = "Angličtina",
                    Description = "Materiály pro AJ",
                    CreatedBy = "Novák",
                    Files = new List<FileDto>
                    {
                        new FileDto { FileName = "Grammar.pdf", FileType = "PDF" },
                        new FileDto { FileName = "Vocabulary.docx", FileType = "Word" }
                    },
                    Links = new List<LinkDto>
                    {
                        new LinkDto { Name = "Online procvičování", Url = "https://example.com/english" }
                    },
                    StudentMaterials = new List<StudentMaterialDto>
                    {
                        new StudentMaterialDto { Title = "Lesson 1", Description = "Úvod do gramatiky" }
                    }
                }
            };

            return Ok(folders);
        }
    }
}
