using System.ComponentModel.DataAnnotations;

namespace JobPortal_New.Modeles.Entites
{
    public class Roles
    {
        [Key]
        public int RoleId { get; set; }

        public string RoleName { get; set; }
    }
}
