using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MIS;

namespace MIS
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
        public DbSet<User> User { get; set; }
        public DbSet<Action> Action { get; set; }
        public DbSet<ActionGroup> ActionGroup { get; set; }
        public DbSet<Samane> Samane { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<UACC> UACC { get; set; }
        public DbSet<RACC> RACC { get; set; }
        public DbSet<Access> Access { get; set; }
        public DbSet<Domain> Domain { get; set; }
        public DbSet<DomainValue> DomainValue { get; set; }
        public DbSet<UACCDomain> UACCDomain { get; set; }
        public DbSet<RACCDomain> RACCDomain { get; set; }
        public DbSet<SubDomainValue> SubDomainValue { get; set; }
        public DbSet<Form> Form { get; set; }
        public DbSet<SubForm> SubForm { get; set; }
        public DbSet<Field> Field { get; set; }
        public DbSet<SubField> SubField { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Access>(
                    eb =>
                    {
                        eb.HasNoKey();
                        eb.ToView("Access");
                    });
            modelBuilder.Entity<SubDomainValue>(
                op =>
                {
                    op.HasOne(p => p.DomainValue).WithMany(d => d.SubDomainValue).HasForeignKey(a=>a.domainvalueid).OnDelete(DeleteBehavior.Cascade);
                    //op.HasOne(p => p.DomainValue).WithMany(d => d.SubDomainValue).HasForeignKey(a=>a.domainvalue2id).OnDelete(DeleteBehavior.Cascade);

                });
            modelBuilder.Entity<SubForm>(
                op =>
                {
                    op.HasOne(p => p.Form1).WithMany(d => d.SubForms1).HasForeignKey(a => a.form1id).OnDelete(DeleteBehavior.Cascade);
                    op.HasOne(p => p.Form2).WithMany(d => d.SubForms2).HasForeignKey(a => a.form2id).OnDelete(DeleteBehavior.Cascade);

                });
        }

        public DbSet<MIS.UserForms> UserForms { get; set; }
    }
}
