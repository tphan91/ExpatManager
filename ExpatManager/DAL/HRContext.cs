using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ExpatManager.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ExpatManager.Models
{
    public class HRContext : DbContext
    {
        //public DbSet<vPersonJob> PersonJobs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}