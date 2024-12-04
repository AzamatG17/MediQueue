using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IStationaryStayRepository : IRepositoryBase<StationaryStayUsage>
    {
        Task<IEnumerable<StationaryStayUsage>> FindAllStationaryStayAsync();
        Task<StationaryStayUsage> FindByIdStationaryStayAsync(int id);
        Task<StationaryStayUsage> FindByIdStationaryAsync(int id);
    }
}
