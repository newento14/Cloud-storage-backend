using Cloud_storage_API.Models;

namespace Cloud_storage_API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateToken(Users user);
        public int ValidateToken(string token);
    }
}
