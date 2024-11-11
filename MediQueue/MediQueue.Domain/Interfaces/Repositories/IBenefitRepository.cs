using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IBenefitRepository : IRepositoryBase<Benefit>
    {
        Task<IEnumerable<Benefit>> FindByIdsAsync(List<int> benefitIds);
    }
}
