using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace api.DataAccess
{
    [DbConfigurationType(typeof(MyDbConfiguration))]
    public partial class ConXContext : DbContext
    //public class ConXContext : DbContext   
    {
        public ConXContext() : base("OracleDbContext")
        {

            //this.Configuration.LazyLoadingEnabled = false;

            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = false;

        }
       
        //public DbSet<whmobileprnt_ctl> mobileprnt_ctl { get; set; }
        //public DbSet<whmobileprnt_default> mobileprnt_def { get; set; }
        //public DbSet<mps_det_wc> mps_wc { get; set; }
        //public DbSet<mps_det_in_process> mps_in_process { get; set; }
        //public DbSet<auth_function> auth { get; set; }
        //public DbSet<mps_mast> mps_mast { get; set; }
        //public DbSet<pd_jit_schedule_ctl> jit_schedule_ctl { get; set; }
        //public DbSet<mps_mr_pcs> mr_pcs { get; set; }
        //public DbSet<mps_det_in_process_tag> mps_tag { get; set; }



        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<whmobileprnt_ctl>().HasKey(t => new { t.SERIES_NO, t.GRP_TYPE });

        //    //modelBuilder
        //    //  .Properties()
        //    //  .Where(p => p.PropertyType == typeof(DateTime))
        //    //  .Configure(p => p.HasPrecision(0));

        //}

       

    }
}