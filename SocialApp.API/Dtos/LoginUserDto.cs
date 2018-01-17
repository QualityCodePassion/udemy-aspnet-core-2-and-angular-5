using System.ComponentModel.DataAnnotations;

namespace SocialApp.API.Dtos
{
    public class LoginUserDto
    {
        [Required]
        public string Username { set; get; }

        [Required]
        public string Password { get; set; }
    }
}