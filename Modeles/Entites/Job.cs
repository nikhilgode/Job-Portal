using System.ComponentModel.DataAnnotations;

namespace JobPortal_New.Modeles.Entites
{
    public class Job
    {
        [Key]
        public int JobId { get; set; }

        public string JobTitle { get; set; }

        public string JobDescription { get; set; }

        public string JobLocation { get; set; }

        public DateTime PostedDate { get; set; }

        public bool IsActive { get; set; }
       
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
