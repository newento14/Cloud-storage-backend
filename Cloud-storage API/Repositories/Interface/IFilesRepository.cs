using Cloud_storage_API.Models;

namespace Cloud_storage_API.Repositories.Interface
{
    public interface IFilesRepository
    {
        Task<Files> AddFileAsync(Files file);
        Task<IEnumerable<Files>> FindByParentIdAsync(int parentId);
        Task<IEnumerable<Files>> FindByUserIdAsync(int id);
    }
}
