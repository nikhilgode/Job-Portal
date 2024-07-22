using System.ComponentModel.DataAnnotations;

namespace JobPortal_New.Modeles.Entites
{
    public class User
    {
       [Key] 
       public int UserId {  get; set; }

       public string UserName { get; set; }

        [EmailAddress]
        [Required]
        public string UserEmail { get; set; }= string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Mobile number must be between 10 and 15 characters.")]
        public string ContactNumber { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; }

        public string Gender { get; set; }

        public Roles roles { get; set; }

    }
}
