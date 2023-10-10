using Cloud_storage_API.Db;
using Cloud_storage_API.Models;
using Cloud_storage_API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Cloud_storage_API.Repositories.Source
{
    public class FilesRepository : IFilesRepository
    {
        private readonly UserContext _db;

        public FilesRepository(UserContext db)
        {
            _db = db;
        }

        public async Task<Files> AddFileAsync(Files file)
        {
            await _db.Files.AddAsync(file);
            await _db.SaveChangesAsync();
            return file;
        }

        public async Task<IEnumerable<Files>> FindByParentIdAsync(int parentId)
        {
            return await _db.Files.Where(x => x.parrentId == parentId).ToListAsync();
        }

        public async Task<IEnumerable<Files>> FindByUserIdAsync(int id)
        {
            return await _db.Files.Where(x => x.UserId == id).ToListAsync();
        }
    }
}
