using System.ComponentModel.DataAnnotations;

namespace SocialApp.API.Dtos
{
    public class RegisterUserDto
    {
        [Required]
        public string Username { get; set; }    

        [Required]
        [StringLength(255, MinimumLength = 4, 
        ErrorMessage = "Must enter a password between 4 and 255 characters long")]
        public string Password { get; set; }
    }
}