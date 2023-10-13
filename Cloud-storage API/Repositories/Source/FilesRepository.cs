using Cloud_storage_API.Db;
using Cloud_storage_API.Models;
using Cloud_storage_API.Repositories.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public void DeleteFile(Files file)
        {
            _db.Files.Remove(file);
            _db.SaveChanges();
        }

        public async Task<Files> FindByIdAsync(int id)
        {
            return await _db.Files.SingleAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Files>> FindByParentIdAsync(int parentId)
        {
            return await _db.Files.Where(x => x.parrentId == parentId).ToListAsync();
        }

        public async Task<IEnumerable<Files>> FindByUserIdAsync(int id)
        {
            return await _db.Files.Where(x => x.UserId == id).ToListAsync();
        }

        public async Task<int> GetLastIdAsync()
        {
            return await _db.Files.OrderByDescending(f => f.Id).Select(f => f.Id).FirstOrDefaultAsync(); 
        }

        public async Task<Files> SetStarredAsync(int id, bool state)
        {
            var file = await _db.Files.SingleAsync(x => x.Id == id);
            if (file != null)
            {
                file.Starred = state;
                await _db.SaveChangesAsync();
                return file;
            }
            return null;
        }
    }
}
