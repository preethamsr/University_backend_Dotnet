using System;
using System.Data.Entity;
namespace University_project_backend.Models
{
    public class Contextclass:DbContext
    {
        public DbSet<User_details> user_Details { get; set; }
        public DbSet<moneytransfer>moneytransfers { get; set; }
        public DbSet<Loan> loans { get; set; }
        public DbSet<Modifyuserdata> modifyuserdatas { get; set; }
        
    }
}
