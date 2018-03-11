using System.ComponentModel.DataAnnotations;

namespace SocialApp.API.Dtos
{
    public class UserForUpdateDto
    {
        public string KnownAs { get; set; }
        public string Introduction { get; set; }
    }
}