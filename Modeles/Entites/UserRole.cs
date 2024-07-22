using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortal_New.Modeles.Entites
{
    public class UserRole
    {
        [Key]
        public int UserRoleId { get; set; }
        public int UserId {  get; set; }
        public int RoleId { get; set; }
    }
}
