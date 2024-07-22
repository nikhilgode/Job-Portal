using System.ComponentModel.DataAnnotations;

namespace JobPortal_New.Modeles.Entites
{
    public class APIOptimize
    {
        [Key]
       public int apiId { get; set; }

       public string ApiName { get; set; }

       public bool IsOptimised { get; set; }
    }
}
