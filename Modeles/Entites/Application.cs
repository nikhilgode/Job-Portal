using System.ComponentModel.DataAnnotations;

namespace JobPortal_New.Modeles.Entites
{
    public class Application
    {
  

        [Key]
        public int JobApplicationId { get; set; }
        public DateTime AppliedDate { get; set; }


        public int UserId { get; set; }

        public User User { get; set; }

        public int JobId { get; set; }

        public Job Job { get; set; }

    }
}
