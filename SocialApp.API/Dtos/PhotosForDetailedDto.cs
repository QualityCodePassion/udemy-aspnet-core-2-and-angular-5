using System;
using System.ComponentModel.DataAnnotations;

namespace SocialApp.API.Dtos
{
    public class PhotosForDetailedDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Url { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        public bool IsMainPhoto { get; set; }
    }
}