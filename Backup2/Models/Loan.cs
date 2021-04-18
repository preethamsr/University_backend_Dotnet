using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University_project_backend.Models
{
    [Table("Loan")]
    public class Loan
    {
        [Key]
        public int LoanID { get; set; }
        public int Income { get; set; }
        public int Co_applicant_income { get; set; }
        public int Age { get; set; }
        public int Loan_amount { get; set; }
        public int Other_emi { get; set; }
        public int Tenure { get; set; }
        public int Account_ID { get; set; }
        public bool Status { get; set; }
        public string Marital_status { get; set; }
        public string Education { get; set; }
        public int Dependents { get; set; }
        public string Employment { get; set; }
        public string Area { get; set; }
        public DateTime Date_of_apply { get; set; }
        public DateTime Date_of_approval { get; set; }
        public string Gender { get; set; }
        public float Credit_History { get; set; }

    }
}
