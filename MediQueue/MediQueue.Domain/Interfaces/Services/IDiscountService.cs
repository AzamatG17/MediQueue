using MediQueue.Domain.DTOs.Discount;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface IDiscountService
    {
        Task<IEnumerable<DiscountDto>> GetAllDiscountsAsync();
        Task<DiscountDto> GetDiscountByIdAsync(int id);
        Task<DiscountDto> CreateDiscountAsync(DiscountForCreateDto discountForCreateDto);
        Task<DiscountDto> UpdateDiscountAsync(DiscountForUpdateDto discountForUpdateDto);
        Task DeleteDiscountAsync(int id);
    }
}
