using System.ComponentModel.DataAnnotations;

namespace WebSample_API.Dtos
{
    public class UserForLoginDto
    {
        [Required(ErrorMessage = "Please! You must input UserName!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please! You must input Password!")]
        public string Password { get; set; }
    }
}