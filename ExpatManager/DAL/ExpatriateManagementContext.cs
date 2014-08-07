using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ExpatManager.Models;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure;

namespace ExpatManager.Models
{


    public partial class ExpatriateManagementContext: DbContext
    {

        //public ExpatriateManagementContext()
        //    : base("name=MVCEntities")
        //{
        //}
        
        public DbSet<AgentDetail> AgentDetails { get; set; }
        public DbSet<AgreementDetail> AgreementDetails { get; set; }
        public DbSet<AgreementDetailDocumentUpload> AgreementDetailDocumentUploads { get; set; }
        public DbSet<AgreementPayment> AgreementPayments { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<CeilingTable> CeilingTables { get; set; }
        public DbSet<DocumentUpload> DocumentUploads { get; set; }
        public DbSet<Expatriate> Expatriates { get; set; }
        public DbSet<ExpatriateDocumentUpload> ExpatriateDocumentUploads { get; set; }
        public DbSet<ExpatriateHistory> ExpatriateHistorys { get; set; }
        public DbSet<Family> Familys { get; set; }
        public DbSet<LandlordBankDetail> LandlordBankDetails { get; set; }
        public DbSet<Parking> Parkings { get; set; }
        public DbSet<vPersonJob> PersonJobs { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Entity<AgreementDetail>()
            //.HasOptional(p => p.LandlordBankDetails).WithRequired(p => p.AgreementDetails);


            
        }

        //partial void OnContextCreated();
    }
}


