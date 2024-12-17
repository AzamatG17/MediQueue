using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories;

public interface IProcedureCategoryRepository : IRepositoryBase<ProcedureCategory>
{
    Task<IEnumerable<ProcedureCategory>> FindAllProcedureCategoryAsync();
    Task<ProcedureCategory> FindByIdProcedureCategoryAsync(int id);
}
