using MediQueue.Domain.DTOs.CategoryLekarstvo;
using MediQueue.Domain.DTOs.Lekarstvo;
using MediQueue.Domain.DTOs.Partiya;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class CategoryLekarstvoService : ICategoryLekarstvoService
{
    private readonly ICategoryLekarstvoRepository _categoryLekarstvoRepository;

    public CategoryLekarstvoService(ICategoryLekarstvoRepository categoryLekarstvoRepository)
    {
        _categoryLekarstvoRepository = categoryLekarstvoRepository ?? throw new ArgumentNullException(nameof(categoryLekarstvoRepository));
    }

    public async Task<IEnumerable<CategoryLekarstvoDto>?> GetAllCategoryLekarstvosAsync()
    {
        var cateforyLekarstvo = await _categoryLekarstvoRepository.FindAllCategoryLekarstvo();

        return cateforyLekarstvo?.Select(MapToCategoryLekarstvoDto).ToList();
    }

    public async Task<CategoryLekarstvoDto> GetCategoryLekarstvoByIdAsync(int id)
    {
        var categoryLekarstvo = await _categoryLekarstvoRepository.FindByIdCategoryLekarstvo(id)
            ?? throw new KeyNotFoundException($"CategoryLekarstvo with id: {id} does not exist.");

        return MapToCategoryLekarstvoDto(categoryLekarstvo);
    }

    public async Task<CategoryLekarstvoDto> CreateCategoryLekarstvoAsync(CategoryLekarstvoForCreateDto lekarstvoForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(lekarstvoForCreateDto);

        var categoryLekarstvo = new CategoryLekarstvo
        {
            Name = lekarstvoForCreateDto.Name,
        };

        await _categoryLekarstvoRepository.CreateAsync(categoryLekarstvo);

        return MapToCategoryLekarstvoDto(categoryLekarstvo);
    }

    public async Task<CategoryLekarstvoDto> UpdateCategoryLekarstvoAsync(CategoryLekarstvoForUpdateDto lekarstvoForUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(lekarstvoForUpdateDto);

        var lakarstvo = new CategoryLekarstvo
        {
            Id = lekarstvoForUpdateDto.Id,
            Name = lekarstvoForUpdateDto.Name
        };

        await _categoryLekarstvoRepository.UpdateAsync(lakarstvo);

        return MapToCategoryLekarstvoDto(lakarstvo);
    }

    public async Task DeleteCategoryLekarstvoAsync(int id)
    {
        await _categoryLekarstvoRepository.DeleteAsync(id);
    }

    private static CategoryLekarstvoDto MapToCategoryLekarstvoDto(CategoryLekarstvo lekarstvo)
    {
        return new CategoryLekarstvoDto(
            lekarstvo.Id,
            lekarstvo.Name,
            lekarstvo.Lekarstvos != null
                ? lekarstvo.Lekarstvos.Select(MapToLekarstvoDto).ToList()
                : new List<LekarstvoDto>()
            );
    }

    private static LekarstvoDto MapToLekarstvoDto(Lekarstvo lekarstvo)
    {
        decimal totalQuantityLEkarstvo = lekarstvo.Partiyas.Sum(x => x.TotalQuantity ?? 0);

        return new LekarstvoDto(
            lekarstvo.Id,
            lekarstvo.Name,
            lekarstvo.PhotoBase64,
            lekarstvo.MeasurementUnit.ToString(),
            lekarstvo.CategoryLekarstvoId,
            lekarstvo.CategoryLekarstvo?.Name ?? "",
            totalQuantityLEkarstvo,
            lekarstvo.Partiyas.Select(p => new PartiyaDto(
                p.Id,
                p.PurchasePrice,
                p.SalePrice,
                p.ExpirationDate,
                p.BeforeDate,
                p.TotalQuantity,
                p.PriceQuantity,
                p.PhotoBase64,
                lekarstvo.MeasurementUnit.ToString(),
                p.LekarstvoId,
                p.Lekarstvo?.Name ?? "",
                p.ScladId,
                p.Sclad?.Name ?? ""
                )).ToList()
            );
    }
}
