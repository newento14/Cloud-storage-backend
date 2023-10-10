using Cloud_storage_API.Db;
using Cloud_storage_API.Models;
using Cloud_storage_API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Cloud_storage_API.Repositories.Source
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _db;

        public UserRepository(UserContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Users>> GetAllAsync()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<Users> GetByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Users> GetByIdAsync(int id)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Users> RegisterAsync(Users user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }
        
    }
}
