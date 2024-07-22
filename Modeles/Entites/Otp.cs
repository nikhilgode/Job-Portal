using System.ComponentModel.DataAnnotations;

namespace JobPortal_New.Modeles.Entites
{
    public class Otp
    {

        [Key]
        public int otpId {  get; set; }

        [Required]
        public string email { get; set; }

        public string otp {  get; set; }

        public DateTime otpSentTime { get; set; }

    }
}
