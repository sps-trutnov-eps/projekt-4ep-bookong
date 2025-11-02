using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookong.Web.Models;

namespace Bookong.Web.Services
{
    public interface ITeachingMaterialsApi
    {
        Task<List<FolderDto>> GetFoldersAsync();
        Task<FolderDto> CreateFolderAsync(FolderDto folder);
    }

    public class TeachingMaterialsApi : ITeachingMaterialsApi
    {
        private readonly List<FolderDto> _folders;

        public TeachingMaterialsApi()
        {
            _folders = new List<FolderDto>
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

        public async Task<List<FolderDto>> GetFoldersAsync()
        {
            await Task.Yield(); // Simulace asynchronního naèítání
            return _folders.ToList();
        }

        public Task<FolderDto> CreateFolderAsync(FolderDto folder)
        {
            folder.Files ??= new List<FileDto>();
            folder.Links ??= new List<LinkDto>();
            folder.StudentMaterials ??= new List<StudentMaterialDto>();

            _folders.Add(folder);
            return Task.FromResult(folder);
        }
    }
}
