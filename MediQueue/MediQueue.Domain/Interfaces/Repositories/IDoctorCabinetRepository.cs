using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IDoctorCabinetRepository : IRepositoryBase<DoctorCabinet>
    {
        Task<IEnumerable<DoctorCabinet>> FindAllDoctorCabinetsAsync();
        Task<DoctorCabinet> FindByIdDoctorCabinetAsync(int id);
    }
}
