using MediQueue.Domain.DTOs.Benefit;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class BenefitService : IBenefitService
{
    private readonly IBenefitRepository _repository;

    public BenefitService(IBenefitRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IEnumerable<BenefitDto>> GetAllBenefitsAsync()
    {
        var benefits = await _repository.FindAllAsync();

        if (benefits == null) return null;

        return benefits.Select(MapToBenefitDto).ToList();
    }

    public async Task<BenefitDto> GetBenefitByIdAsync(int id)
    {
        var benefit = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"Benefit with {id} not found");

        return MapToBenefitDto(benefit);
    }

    public async Task<BenefitDto> CreateBenefitAsync(BenefitForCreateDto benefitForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(benefitForCreateDto));

        if (benefitForCreateDto.Percent is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(benefitForCreateDto.Percent), "Benefit percent must be between 0 and 100.");
        }

        var benefit = new Benefit
        {
            Name = benefitForCreateDto.Name,
            Percent = benefitForCreateDto.Percent,
        };

        await _repository.CreateAsync(benefit);

        return MapToBenefitDto(benefit);
    }

    public async Task<BenefitDto> UpdateBenefitAsync(BenefitForUpdateDto benefitForUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(benefitForUpdateDto));

        if (benefitForUpdateDto.Percent is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(benefitForUpdateDto.Percent), "Benefit percent must be between 0 and 100.");
        }

        var benefit = await _repository.FindByIdAsync(benefitForUpdateDto.Id);
        if (benefit == null)
            throw new KeyNotFoundException($"Benefit with id: {benefitForUpdateDto.Id} does not exist!");

        benefit.Name = benefitForUpdateDto.Name;
        benefit.Percent = benefitForUpdateDto.Percent;

        await _repository.UpdateAsync(benefit);

        return MapToBenefitDto(benefit);
    }

    public async Task DeleteBenefitAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private static BenefitDto MapToBenefitDto(Benefit benefit)
    {
        return new BenefitDto(
            benefit.Id,
            benefit.Name,
            benefit.Percent
            );
    }
}
