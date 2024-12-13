using MediQueue.Domain.DTOs.Discount;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class DiscountService : IDiscountService
{
    private readonly IDiscountRepository _repository;

    public DiscountService(IDiscountRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IEnumerable<DiscountDto>> GetAllDiscountsAsync()
    {
        var discounts = await _repository.FindAllAsync();

        if (discounts == null) return null;

        return discounts.Select(MapToDiscountDto).ToList();
    }

    public async Task<DiscountDto> GetDiscountByIdAsync(int id)
    {
        var discount = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"Discount with id: {id} does not exist.");

        return MapToDiscountDto(discount);
    }

    public async Task<DiscountDto> CreateDiscountAsync(DiscountForCreateDto discountForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(discountForCreateDto));

        if (discountForCreateDto.Percent is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(discountForCreateDto.Percent), "Discount percent must be between 0 and 100.");
        }

        var discount = new Discount
        {
            Name = discountForCreateDto.Name,
            Percent = discountForCreateDto.Percent,
        };

        await _repository.CreateAsync(discount);

        return MapToDiscountDto(discount);
    }

    public async Task<DiscountDto> UpdateDiscountAsync(DiscountForUpdateDto discountForUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(discountForUpdateDto));

        if (discountForUpdateDto.Percent is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(discountForUpdateDto.Percent), "Discount percent must be between 0 and 100.");
        }

        var discount = await _repository.FindByIdAsync(discountForUpdateDto.Id);

        if (discount == null) throw new KeyNotFoundException($"Discount with id: {discount} does not exist!");

        discount.Name = discountForUpdateDto.Name;
        discount.Percent = discountForUpdateDto.Percent;

        await _repository.UpdateAsync(discount);

        return MapToDiscountDto(discount);
    }

    public async Task DeleteDiscountAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private static DiscountDto MapToDiscountDto(Discount discount)
    {
        return new DiscountDto(
            discount.Id,
            discount.Name,
            discount.Percent
            );
    }
}
