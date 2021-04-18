using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University_project_backend.Models
{
    [Table("User_detail")]
    public class User_details
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime DOB { get; set; }
        public string Account_number { get; set; }
        public string Account_type { get; set; }
        public DateTime Date_of_creation { get; set; }
        public string Email_address { get; set; }
        public string Activationcode { get; set; }
        public string Verified { get; set; }
        public int Balance { get; set; }
        public string Loan { get; set; }
        public int Loan_amount { get; set; }
        public string Loan_status { get; set; }
        
    }
}
