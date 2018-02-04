using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SocialApp.API.Models;

namespace SocialApp.API.Dtos
{
    public class UserForDetailed
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

        public string Introduction { get; set; }

        public string Likes { get; set; }

        public string Interests { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string PhotoUrl { get; set; }

        public ICollection<PhotosForDetailedDto> Photos { get; set; }
    }
}