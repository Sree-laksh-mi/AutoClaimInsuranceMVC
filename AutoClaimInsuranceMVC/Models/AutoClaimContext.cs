using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AutoClaimInsuranceMVC.Models
{
    public class AutoClaimContext:DbContext
    {
        public AutoClaimContext():base("Name=Dbconfig")
        {

        }
        public DbSet<Insurer> Insurers { get; set; }
        public DbSet<Insurance> Insurances{ get; set; }
        public DbSet<RegisteredUser> registeredUsers { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Officer> Officers { get; set; }
        public DbSet<Report> Reports { get; set; }


    }

}