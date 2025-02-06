using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace PundoPH.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Token { get; set; } = Guid.NewGuid().ToString();
        public DateTime TokenExpirationDate { get; set; } = DateTime.Now;
    }
}
