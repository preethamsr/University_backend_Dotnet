using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University_project_backend.Models
{
    [Table("Transaction")]
    public class moneytransfer
    {
            [Key]
            public int Transaction_ID { get; set; }           
            public string To_account { get; set; }
            public int Amount { get; set; }
            public DateTime Date { get; set; }
            public string Beneficiary { get; set; }
            public int Account_id { get; set; }            
        
    }
}
