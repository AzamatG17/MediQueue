using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MediQueue.Infrastructure.Persistence
{
    public class MediQueueDbContext : DbContext
    {
        public virtual  DbSet<Role> Roles { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Questionnaire> Questionnaires { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<QuestionnaireHistory> QuestionnaireHistories { get; set; }
        public virtual DbSet<PaymentService> PaymentServices { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<CategoryLekarstvo> CategoryLekarstvos { get; set; }
        public virtual DbSet<Sclad> Sclads { get; set; }
        public virtual DbSet<Lekarstvo> Lekarstvos { get; set; }

        public MediQueueDbContext(DbContextOptions<MediQueueDbContext> options)
            :base(options)
        {
            //Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Permission>().HasData(
            new Permission { Id = 1, Name = "Get" },
            new Permission { Id = 2, Name = "GetById" },
            new Permission { Id = 3, Name = "Post" },
            new Permission { Id = 4, Name = "Put" },
            new Permission { Id = 5, Name = "Delete" }
        );
        }
    }
}
