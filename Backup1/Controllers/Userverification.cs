using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University_project_backend.Models;
namespace University_project_backend.Controllers
{
    public class UserverificationController : Controller
    {
        public ActionResult Userverification(string activationcode)
        {
           using(Contextclass contextclass=new Contextclass())
            {
                 var activationcode_user = contextclass.user_Details.Where(x => x.Activationcode == activationcode).FirstOrDefault();
                 activationcode_user.Verified = "YES";
                 contextclass.SaveChanges();

                return View();
            }
            
        }
    }
}
