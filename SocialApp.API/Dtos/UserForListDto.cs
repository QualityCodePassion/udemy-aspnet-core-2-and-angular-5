using System;
using System.ComponentModel.DataAnnotations;

namespace SocialApp.API.Dtos
{
    public class UserForListDto
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string Username { get; set; }

        public string Gender { get; set; }

        [Required]
        public int Age { get; set; }

        public string KnownAs { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime LastActive { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string PhotoUrl { get; set; }

    }
}