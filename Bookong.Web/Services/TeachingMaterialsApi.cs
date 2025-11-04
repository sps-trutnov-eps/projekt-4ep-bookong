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
        Task UpdateFolderAsync(FolderDto folder);  // EDIT
        Task DeleteFolderAsync(FolderDto folder);  // DELETE
    }

    public class TeachingMaterialsApi : ITeachingMaterialsApi
    {
        private readonly List<FolderDto> _folders;

        public TeachingMaterialsApi()
        {
            _folders = new List<FolderDto>();
        }

        public async Task<List<FolderDto>> GetFoldersAsync()
        {
            await Task.Yield();
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

        public Task UpdateFolderAsync(FolderDto folder)
        {
            var existing = _folders.FirstOrDefault(f => f.Name == folder.Name && f.CreatedBy == folder.CreatedBy);
            if (existing != null)
            {
                existing.Description = folder.Description;
                
            }
            return Task.CompletedTask;
        }

        public Task DeleteFolderAsync(FolderDto folder)
        {
            var existing = _folders.FirstOrDefault(f => f.Name == folder.Name && f.CreatedBy == folder.CreatedBy);
            if (existing != null)
            {
                _folders.Remove(existing);
            }
            return Task.CompletedTask;
        }
    }
}
