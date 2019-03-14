using System.ComponentModel.DataAnnotations;

namespace WebSample_API.Dtos
{
    public class UserForRegisterDto
    {
        [Required(ErrorMessage = "Please! You must input this field!")]
        public string UserName { get; set; }

        [StringLength(50, MinimumLength = 5, ErrorMessage = "Please! You must input between 5 and 50 characters!")]
        public string Password { get; set; }
    }
}