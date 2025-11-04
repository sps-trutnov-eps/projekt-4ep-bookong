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
    }
}
