using MediQueue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediQueue.Infrastructure.Persistence.Configurations
{
    internal class LekarstvoConfiguration : IEntityTypeConfiguration<Lekarstvo>
    {
        public void Configure(EntityTypeBuilder<Lekarstvo> builder)
        {
            throw new NotImplementedException();
        }
    }
}
