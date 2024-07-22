using System.ComponentModel.DataAnnotations;

namespace JobPortal_New.Dto
{
    public class UserLoginDto
    {
        [EmailAddress]
        [Required]
        public string UserEmail { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
