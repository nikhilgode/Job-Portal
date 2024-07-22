using System.ComponentModel.DataAnnotations;

namespace JobPortal_New.Dto
{
    public class JobDto
    {
       

        [Required]
        public string JobTitle { get; set; }

        public string JobDescription { get; set; }

        [Required]
        public string JobLocation { get; set; }

        public DateTime PostedDate { get; set; }


        public int UserId { get; set; }

    }
}
