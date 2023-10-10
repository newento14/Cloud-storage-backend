namespace Cloud_storage_API.Models.Dtos
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public long StorageSize { get; set; }
        public long StorageUsed { get; set; }
    }
}
