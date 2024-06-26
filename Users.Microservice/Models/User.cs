using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Users.Microservice.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [JsonIgnore]
        public string? PasswordHash { get; set; }
    }
}
