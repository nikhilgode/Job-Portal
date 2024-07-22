using System.ComponentModel.DataAnnotations;

namespace JobPortal_New.Dto
{
    public class UserDto
    {

        [Required]
        public string Name { get; set; }

        public string Gender { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        public int role { get; set; }
    }
}
