using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface ICategoryLekarstvoRepository : IRepositoryBase<CategoryLekarstvo>
    {
        Task<IEnumerable<CategoryLekarstvo>> FindAllCategoryLekarstvo();
        Task<CategoryLekarstvo> FindByIdCategoryLekarstvo(int id);
    }
}
