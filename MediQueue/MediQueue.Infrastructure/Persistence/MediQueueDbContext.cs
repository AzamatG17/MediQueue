using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MediQueue.Infrastructure.Persistence
{
    public class MediQueueDbContext : DbContext
    {
        public virtual  DbSet<Role> Roles { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }

        public MediQueueDbContext(DbContextOptions<MediQueueDbContext> options)
            :base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
