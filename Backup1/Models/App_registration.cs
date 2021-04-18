using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace University_project_backend.Models
{
    [Table("App_registration")]
    public class App_registration
    {
        [Key]   
        public Guid App_id { get; set; }
        public string App_name { get; set; } 
        public string URL { get; set; }        
        public string Email_to_contact { get; set; }
        public Guid Secret_key { get; set; }

    }
}