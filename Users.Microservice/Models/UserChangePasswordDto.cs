namespace Users.Microservice.Models
{
    public class UserChangePasswordDto
    {
        public string? Email { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
