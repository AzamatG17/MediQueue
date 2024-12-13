using MediQueue.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;
using System.Security.Claims;

namespace MediQueue.Infrastructure.Persistence
{
    public class MediQueueDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        #region DbSet   
        public virtual DbSet<Role> Roles { get; set; }
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
        public virtual DbSet<Controller> Controllers { get; set; }
        public virtual DbSet<AccountSession> AccountSessions { get; set; }
        public virtual DbSet<Conclusion> Conclusion { get; set; }
        public virtual DbSet<LekarstvoUsage> LekarstvoUsages { get; set; }
        public virtual DbSet<ServiceUsage> ServiceUsages { get; set; }
        public virtual DbSet<AnalysisResult> AnalysisResults { get; set; }
        public virtual DbSet<DoctorCabinet> DoctorCabinets { get; set; }
        public virtual DbSet<DoctorCabinetLekarstvo> DoctorCabinetLekarstvos { get; set; }
        public virtual DbSet<Partiya> Partiyas { get; set; }
        public virtual DbSet<Sample> Samples { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<Benefit> Benefits { get; set; }
        public virtual DbSet<Nutrition> Nutritions { get; set; }
        public virtual DbSet<StationaryStayUsage> StationaryStays { get; set; }
        public virtual DbSet<Tariff> Tariffs { get; set; }
        public virtual DbSet<Ward> Wards { get; set; }
        public virtual DbSet<WardPlace> WardsPlace { get; set; }
        public virtual DbSet<AuditLog> AuditLogs { get; set; }
        #endregion

        public MediQueueDbContext(DbContextOptions<MediQueueDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            //Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var auditEntries = PrepareAuditEntries();
            var result = base.SaveChanges();
            FinalizeAuditEntries(auditEntries);
            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var auditEntries = PrepareAuditEntries();
            var result = await base.SaveChangesAsync(cancellationToken);
            FinalizeAuditEntries(auditEntries);
            return result;
        }

        private List<AuditEntry> PrepareAuditEntries()
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = CreateAuditEntry(entry);
                auditEntries.Add(auditEntry);
            }

            foreach (var auditEntry in auditEntries.Where(e => !e.HasTemporaryProperties))
            {
                AuditLogs.Add(auditEntry.ToAuditLog());
            }

            return auditEntries.Where(e => e.HasTemporaryProperties).ToList();
        }

        private AuditEntry CreateAuditEntry(EntityEntry entry)
        {
            var auditEntry = new AuditEntry(entry)
            {
                TableName = entry.Entity.GetType().Name,
                Action = entry.State.ToString(),
                UserId = (int)GetCurrentAccountId()
            };

            foreach (var property in entry.Properties)
            {
                var propertyName = property.Metadata.Name;

                if (property.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[propertyName] = property.CurrentValue ?? property.OriginalValue;
                }

                if (property.IsTemporary)
                {
                    auditEntry.TemporaryProperties.Add(property);
                    continue;
                }

                if (entry.State == EntityState.Added)
                {
                    auditEntry.NewValues[propertyName] = property.CurrentValue;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    auditEntry.OldValues[propertyName] = property.OriginalValue;
                }
                else if (entry.State == EntityState.Modified && property.IsModified)
                {
                    auditEntry.OldValues[propertyName] = property.OriginalValue;
                    auditEntry.NewValues[propertyName] = property.CurrentValue;
                }
            }

            return auditEntry;
        }

        private void FinalizeAuditEntries(List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0) return;

            foreach (var auditEntry in auditEntries)
            {
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }

                AuditLogs.Add(auditEntry.ToAuditLog());
            }

            base.SaveChanges();
        }

        private int? GetCurrentAccountId()
        {
            var userClaims = _httpContextAccessor.HttpContext?.User?.Claims;
            var accountIdClaim = userClaims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            return int.TryParse(accountIdClaim, out var accountId) ? accountId : null;
        }
    }
}
