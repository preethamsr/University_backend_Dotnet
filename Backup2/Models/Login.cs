using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace University_project_backend.Models
{
    
    public class Login
    {
       public string Email_address { set; get; }
        public string Password { set; get; }
    }
}
