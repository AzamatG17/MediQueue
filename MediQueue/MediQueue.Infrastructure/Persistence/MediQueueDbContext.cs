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
        public virtual DbSet<Controllers> Controllers { get; set; }
        public virtual DbSet<AccountSession> AccountSessions { get; set; }
        public virtual DbSet<Conclusion> Conclusion { get; set; }
        public virtual DbSet<PaymentLekarstvo> PaymentLekarstvos { get; set; }
        public virtual DbSet<LekarstvoUsage> LekarstvoUsages { get; set; }
        public virtual DbSet<ServiceUsage> ServiceUsages { get; set; }
        public virtual DbSet<AnalysisResult> AnalysisResults { get; set; }
        public virtual DbSet<DoctorCabinet> DoctorCabinets { get; set; }
        public virtual DbSet<DoctorCabinetLekarstvo> DoctorCabinetLekarstvos { get; set; }
        public virtual DbSet<Partiya> Partiyas { get; set; }

        public MediQueueDbContext(DbContextOptions<MediQueueDbContext> options)
            :base(options)
        {
            //Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
