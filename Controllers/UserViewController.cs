using JobPortal_New.Interfaces.Repositories;
using JobPortal_New.Modeles.Entites;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal_New.Controllers
{
    public class UserViewController : Controller
    {
        private readonly IUserRepository _iUser;
        public UserViewController(IUserRepository iUser)
        {
            _iUser = iUser;

        }

      



    }
}
