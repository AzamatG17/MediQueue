using MediQueue.Domain.Entities;
using MediQueue.Domain.ResourceParameters;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface ILekarstvoRepository : IRepositoryBase<Lekarstvo>
    {
        Task<IEnumerable<Lekarstvo>> FindAllLekarstvoAsync(LekarstvoResourceParametrs lekarstvoResourceParametrs);
        Task<Lekarstvo> FindByIdLekarstvoAsync(int id);
    }
}
