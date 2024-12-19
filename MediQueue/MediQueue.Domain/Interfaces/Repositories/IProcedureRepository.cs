using MediQueue.Domain.Entities;
using MediQueue.Domain.ResourceParameters;

namespace MediQueue.Domain.Interfaces.Repositories;

public interface IProcedureRepository : IRepositoryBase<Procedure>
{
    Task<IEnumerable<Procedure>> FindAllProcedureAsync(ProcedureResourceParameters procedureResourceParameters);
    Task<Procedure> FindByIdProcedureAsync(int id);
}
