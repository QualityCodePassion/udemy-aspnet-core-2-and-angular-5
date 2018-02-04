using System;
using System.ComponentModel.DataAnnotations;

namespace SocialApp.API.Models
{
    public class Photo
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Url { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        public bool IsMainPhoto { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }

    }
}