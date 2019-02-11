using LOBCore.DataAccesses.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LOBCore.DataAccesses
{
    public class LOBDatabaseContext : DbContext
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual DbSet<LobUser> LobUsers { get; set; }
        public virtual DbSet<LeaveForm> LeaveForms { get; set; }
        public virtual DbSet<LeaveFormType> LeaveFormTypes { get; set; }
        public virtual DbSet<ExceptionRecord> ExceptionRecords { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<CommUserGroup> CommUserGroups { get; set; }
        public virtual DbSet<CommUserGroupItem> CommUserGroupItems { get; set; }
        public virtual DbSet<NotificationToken> NotificationTokens { get; set; }
        public virtual DbSet<SystemEnvironment> SystemEnvironment { get; set; }
        public LOBDatabaseContext(DbContextOptions<LOBDatabaseContext> options)
               : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
