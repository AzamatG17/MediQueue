using MediQueue.Domain.DTOs.CategoryLekarstvo;
using MediQueue.Domain.DTOs.Lekarstvo;
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

    public async Task<IEnumerable<CategoryLekarstvoDto>> GetAllCategoryLekarstvosAsync()
    {
        var cateforyLekarstvo = await _categoryLekarstvoRepository.FindAllCategoryLekarstvo();

        return cateforyLekarstvo.Select(MapToCategoryLekarstvoDto).ToList();
    }

    public async Task<CategoryLekarstvoDto> GetCategoryLekarstvoByIdAsync(int id)
    {
        var categoryLekarstvo = await _categoryLekarstvoRepository.FindByIdCategoryLekarstvo(id);

        return MapToCategoryLekarstvoDto(categoryLekarstvo);
    }

    public async Task<CategoryLekarstvoDto> CreateCategoryLekarstvoAsync(CategoryLekarstvoForCreateDto lekarstvoForCreateDto)
    {
        if (lekarstvoForCreateDto == null)
        {
            throw new ArgumentNullException(nameof(lekarstvoForCreateDto));
        }

        var categoryLekarstvo = new CategoryLekarstvo
        {
            Name = lekarstvoForCreateDto.Name,
        };

        await _categoryLekarstvoRepository.CreateAsync(categoryLekarstvo);

        return MapToCategoryLekarstvoDto(categoryLekarstvo);
    }

    public async Task<CategoryLekarstvoDto> UpdateCategoryLekarstvoAsync(CategoryLekarstvoForUpdateDto lekarstvoForUpdateDto)
    {
        if (lekarstvoForUpdateDto == null)
        {
            throw new ArgumentNullException(nameof(lekarstvoForUpdateDto));
        }

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

    private CategoryLekarstvoDto MapToCategoryLekarstvoDto(CategoryLekarstvo lekarstvo)
    {
        return new CategoryLekarstvoDto(
            lekarstvo.Id,
            lekarstvo.Name,
            lekarstvo.Lekarstvos != null
                ? lekarstvo.Lekarstvos.Select(MapToLekarstvoDto).ToList()
                : new List<LekarstvoDto>()
            );
    }

    private LekarstvoDto MapToLekarstvoDto(Lekarstvo lekarstvo)
    {
        return new LekarstvoDto(
            lekarstvo.Id,
            lekarstvo.Name,
            lekarstvo.PhotoBase64,
            lekarstvo.CategoryLekarstvoId,
            lekarstvo.CategoryLekarstvo.Name ?? null
            );
    }
}
