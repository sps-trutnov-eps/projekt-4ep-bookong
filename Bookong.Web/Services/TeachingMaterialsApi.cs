using Bookong.Web.Models;
using static Bookong.Web.Components.Pages.TeachingMaterials;

namespace Bookong.Web.Services
{
    public class TeachingMaterialsApi : ITeachingMaterialsApi
    {
        public async Task<List<FolderDto>> GetFoldersAsync()
        {
            // Simulace asynchronní operace
            await Task.Yield();
            return new List<FolderDto>
            {
                new FolderDto
                {
                    Name = "Angliètina",
                    Description = "Materiály pro AJ",
                    CreatedBy = "Novák",
                    Files = new List<FileDto>(),
                    Links = new List<LinkDto>(),
                    StudentMaterials = new List<StudentMaterialDto>()
                }
            };
        }
    }

    public interface ITeachingMaterialsApi
    {
        Task<List<FolderDto>> GetFoldersAsync();
    }
}
