using Bookong.Web.Models;

namespace Bookong.Web.Services
{
    public interface ITeachingMaterialsApi
    {
        Task<List<FolderDto>> GetFoldersAsync();
    }

    public class TeachingMaterialsApi : ITeachingMaterialsApi
    {
        public async Task<List<FolderDto>> GetFoldersAsync()
        {
            await Task.Yield(); // Simulace asynchronního naèítání

            return new List<FolderDto>
            {
                new FolderDto
                {
                    Name = "Angliètina",
                    Description = "Materiály pro AJ",
                    CreatedBy = "Novák",
                    Files = new List<FileDto>
                    {
                        new FileDto { FileName = "Slovíèka.pdf", FileType = "PDF" }
                    },
                    Links = new List<LinkDto>
                    {
                        new LinkDto { Name = "Online cvièení", Url = "https://example.com/cviceni" }
                    },
                    StudentMaterials = new List<StudentMaterialDto>
                    {
                        new StudentMaterialDto { Title = "Prezentace", Description = "Základy gramatiky" }
                    }
                },
                new FolderDto
                {
                    Name = "Matematika",
                    Description = "Materiály pro Matematiku",
                    CreatedBy = "Dvoøák",
                    Files = new List<FileDto>
                    {
                        new FileDto { FileName = "Písemka.docx", FileType = "Word" }
                    },
                    Links = new List<LinkDto>
                    {
                        new LinkDto { Name = "Matematika online", Url = "https://example.com/matematika" }
                    },
                    StudentMaterials = new List<StudentMaterialDto>
                    {
                        new StudentMaterialDto { Title = "Cvièení", Description = "Sèítání a odèítání" }
                    }
                }
            };
        }
    }
}
