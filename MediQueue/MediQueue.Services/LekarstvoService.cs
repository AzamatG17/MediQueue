using MediQueue.Domain.DTOs.Lekarstvo;
using MediQueue.Domain.DTOs.Partiya;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Entities.Enums;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class LekarstvoService : ILekarstvoService
{
    private readonly ILekarstvoRepository _repository;
    private readonly ICategoryLekarstvoRepository _categoryLekarstvoRepository;

    public LekarstvoService(ILekarstvoRepository repository, ICategoryLekarstvoRepository categoryLekarstvoRepository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _categoryLekarstvoRepository = categoryLekarstvoRepository ?? throw new ArgumentNullException(nameof(categoryLekarstvoRepository));
    }

    public async Task<IEnumerable<LekarstvoDto>> GetAllLekarstvosAsync()
    {
        var lekarstvo = await _repository.FindAllLekarstvoAsync();

        if (lekarstvo == null) return null;

        return lekarstvo.Select(MapToLekarstvoDto).ToList();
    }

    public async Task<LekarstvoDto> GetLekarstvoByIdAsync(int id)
    {
        var lekarstvo = await _repository.FindByIdLekarstvoAsync(id);

        if (lekarstvo == null) return null;

        return MapToLekarstvoDto(lekarstvo);
    }

    public async Task<LekarstvoDto> CreateLekarstvoAsync(LekarstvoForCreateDto lekarstvoForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(lekarstvoForCreateDto));

        if (! await _categoryLekarstvoRepository.IsExistByIdAsync(lekarstvoForCreateDto.CategoryLekarstvoId))
            throw new ArgumentException($"CategoryLekarstvo with id: {lekarstvoForCreateDto.CategoryLekarstvoId} does not exist");

        var lekarstvo = await MapToLekarstvo(lekarstvoForCreateDto);

        await _repository.CreateAsync(lekarstvo);

        return MapToLekarstvoDto(lekarstvo);
    }

    public async Task<LekarstvoDto> UpdateLekarstvoAsync(LekarstvoForUpdateDto lekarstvoForUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(lekarstvoForUpdateDto));

        if (!await _categoryLekarstvoRepository.IsExistByIdAsync(lekarstvoForUpdateDto.CategoryLekarstvoId))
            throw new ArgumentException($"CategoryLekarstvo with id: {lekarstvoForUpdateDto.CategoryLekarstvoId} does not exist");

        var lekarstvo = await MapToLekarstvoUpdate(lekarstvoForUpdateDto);

        await _repository.UpdateAsync(lekarstvo);

        return MapToLekarstvoDto(lekarstvo);
    }

    public async Task DeleteLekarstvoAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private async Task<Lekarstvo> MapToLekarstvoUpdate(LekarstvoForUpdateDto lekarstvo)
    {
        return new Lekarstvo
        {
            Id = lekarstvo.Id,
            Name = lekarstvo.Name,
            PhotoBase64 = lekarstvo.PhotoBase64,
            MeasurementUnit = lekarstvo.MeasurementUnit,
            CategoryLekarstvoId = lekarstvo.CategoryLekarstvoId
        };
    }

    private async Task<Lekarstvo> MapToLekarstvo(LekarstvoForCreateDto lekarstvo)
    {
        return new Lekarstvo
        {
            Name = lekarstvo.Name,
            PhotoBase64 = lekarstvo.PhotoBase64,
            MeasurementUnit = lekarstvo.MeasurementUnit,
            CategoryLekarstvoId = lekarstvo.CategoryLekarstvoId
        };
    }

    private LekarstvoDto MapToLekarstvoDto(Lekarstvo lekarstvo)
    {
        decimal totalQuantityLEkarstvo = lekarstvo.Partiyas?.Sum(x => x.TotalQuantity ?? 0) ?? 0;

        var measurementUnit = lekarstvo.MeasurementUnit.HasValue
        ? Enum.GetName(typeof(MeasurementUnit), lekarstvo.MeasurementUnit) ?? ""
        : "";

        return new LekarstvoDto(
            lekarstvo.Id,
            lekarstvo.Name,
            lekarstvo.PhotoBase64,
            measurementUnit,
            lekarstvo.CategoryLekarstvoId,
            lekarstvo.CategoryLekarstvo?.Name ?? "",
            totalQuantityLEkarstvo,
            lekarstvo.Partiyas?.Select(p => new PartiyaDto(
                p.Id,
                p.PurchasePrice,
                p.SalePrice,
                p.ExpirationDate,
                p.BeforeDate,
                p.TotalQuantity,
                p.PriceQuantity,
                p.PhotoBase64,
                p.LekarstvoId,
                p.Lekarstvo?.Name ?? "",
                p.ScladId,
                p.Sclad?.Name ?? ""
            )).ToList() ?? new List<PartiyaDto>()
        );
    }
}
