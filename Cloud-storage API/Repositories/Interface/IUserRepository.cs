using Cloud_storage_API.Models;
using Cloud_storage_API.Models.Dtos;

namespace Cloud_storage_API.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<Users> RegisterAsync(Users user);
        Task<IEnumerable<Users>> GetAllAsync();
        Task<Users> GetByIdAsync(int id);
        Task<Users> GetByEmailAsync(string email);
    }
}
