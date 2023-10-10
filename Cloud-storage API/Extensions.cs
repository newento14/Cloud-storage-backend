using Cloud_storage_API.Models.Dtos;
using Cloud_storage_API.Models;

namespace Cloud_storage_API
{
    public static class Extensions
    {
        public static UsersDto AsDto(this Users item)
        {
            return new UsersDto()
            {
                Id = item.Id,
                Email = item.Email,
                Password = item.Password

            };
        }
    }
}
