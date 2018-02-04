using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SocialApp.API.Models
{
    public class User
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string Username { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte [] PasswordSalt { get; set; }

        public string Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

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

        public ICollection<Photo> Photos { get; set; }

        public User() {
            Photos = new Collection<Photo>();
        }        

    }
}