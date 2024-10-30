using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class DoctorCabinetRepository : RepositoryBase<DoctorCabinet>, IDoctorCabinetRepository
    {
        public DoctorCabinetRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }
    }
}
