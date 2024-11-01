using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IDoctorCabinetLekarstvoRepository : IRepositoryBase<DoctorCabinetLekarstvo>
    {
        Task<IEnumerable<DoctorCabinetLekarstvo>> FindAllDoctorCabinetLekarstvoAsync();
        Task<DoctorCabinetLekarstvo> FindByIdDoctorCabinetLekarstvoAsync(int id);
    }
}
