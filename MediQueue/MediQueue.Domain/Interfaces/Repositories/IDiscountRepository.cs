using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Repositories
{
    public interface IDiscountRepository : IRepositoryBase<Discount>
    {
        Task<IEnumerable<Discount>> FindByIdsAsync(List<int> discountIds);
    }
}
