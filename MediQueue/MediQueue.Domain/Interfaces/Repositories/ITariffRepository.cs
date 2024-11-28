using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface ITariffRepository : IRepositoryBase<Tariff>
    {
        Task<IEnumerable<Tariff>> FindByIdsAsync(List<int> ids);
        Task<IEnumerable<Tariff>> FindAllTariffAsync();
        Task<Tariff> FindByIdTariffAsync(int id);
    }
}
