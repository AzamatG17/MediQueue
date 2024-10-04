using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    public class AccountSessionConfiguration : IEntityTypeConfiguration<AccountSession>
    {
        public void Configure(EntityTypeBuilder<AccountSession> builder)
        {
            builder.ToTable(nameof(AccountSession));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.AccountId)
                .IsRequired();

            builder.Property(x => x.LastActivitytime);

            builder.Property(x => x.RefreshTokenExpiry);

            builder.Property(x => x.IsLoggedOut)
                .IsRequired();

            builder.Property(x => x.SessionId)
                .IsRequired();

            builder.Property(x => x.AccessToken)
                .IsRequired();
        }
    }
}
