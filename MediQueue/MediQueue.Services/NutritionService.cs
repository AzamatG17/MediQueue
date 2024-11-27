using AutoMapper;
using MediQueue.Domain.DTOs.Nutrition;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class NutritionService : INutritionService
{
    private readonly INutritionRepository _repository;
    private readonly IMapper _mapper;

    public NutritionService(INutritionRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<NutritionDto>> GetAllNutritionsAsync()
    {
        var nutritions = await _repository.FindAllAsync();

        if (nutritions == null) return null;

        return _mapper.Map<IEnumerable<NutritionDto>>(nutritions);
    }

    public async Task<NutritionDto> GetNutritionByIdAsync(int id)
    {
        var nutrition = await _repository.FindByIdAsync(id);

        if (nutrition == null) return null;

        return _mapper.Map<NutritionDto>(nutrition);
    }

    public async Task<NutritionDto> CreateNutritionAsync(NutritionForCreateDto nutritionForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(nutritionForCreateDto));

        var nutrition = _mapper.Map<Nutrition>(nutritionForCreateDto);

        await _repository.CreateAsync(nutrition);

        return _mapper.Map<NutritionDto>(nutrition);
    }

    public async Task<NutritionDto> UpdateNutritionAsync(NutritionForUpdateDto nutritionForUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(nutritionForUpdateDto));

        var nutrition = await _repository.FindByIdAsync(nutritionForUpdateDto.Id)
            ?? throw new KeyNotFoundException($"Nutrition with id: {nutritionForUpdateDto.Id} does not exist.");

        nutrition.MealPlan = nutritionForUpdateDto.MealPlan;
        nutrition.CostPerDay = nutritionForUpdateDto.CostPerDay;

        await _repository.UpdateAsync(nutrition);

        return _mapper.Map<NutritionDto>(nutrition);
    }

    public async Task DeleteNutritionAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}
