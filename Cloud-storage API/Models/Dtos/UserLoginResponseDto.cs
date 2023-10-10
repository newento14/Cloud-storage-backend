namespace Cloud_storage_API.Models.Dtos
{
    public class UserLoginResponseDto
    {
        public UserResponse User { get; set; }
        public string Token { get; set; }
    }
}
