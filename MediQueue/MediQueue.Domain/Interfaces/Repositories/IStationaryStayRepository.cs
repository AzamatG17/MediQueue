using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IStationaryStayRepository : IRepositoryBase<StationaryStay>
    {
        Task<IEnumerable<StationaryStay>> FindAllStationaryStayAsync();
        Task<StationaryStay> FindByIdStationaryStayAsync(int id);
    }
}
