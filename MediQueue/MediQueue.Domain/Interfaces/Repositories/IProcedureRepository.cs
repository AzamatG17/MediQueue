using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories;

public interface IProcedureRepository : IRepositoryBase<Procedure>
{
    Task<IEnumerable<Procedure>> FindAllProcedureAsync();
    Task<Procedure> FindByIdProcedureAsync(int id);
}
