using MediQueue.Domain.DTOs.Lekarstvo;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class LekarstvoService : ILekarstvoService
{
    private readonly ILekarstvoRepository _repository;

    public LekarstvoService(ILekarstvoRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IEnumerable<LekarstvoDto>> GetAllLekarstvosAsync()
    {
        var lekarstvo = await _repository.FindAllAsync();

        return lekarstvo.Select(MapToLekarstvoDto).ToList();
    }

    public async Task<LekarstvoDto> GetLekarstvoByIdAsync(int id)
    {
        var lekarstvo = await _repository.FindByIdAsync(id);

        return MapToLekarstvoDto(lekarstvo);
    }

    public async Task<LekarstvoDto> CreateLekarstvoAsync(LekarstvoForCreateDto lekarstvoForCreateDto)
    {
        if (lekarstvoForCreateDto == null)
        {
            throw new ArgumentNullException(nameof(lekarstvoForCreateDto));
        }

        var lekarstvo = await MapToLekarstvo(lekarstvoForCreateDto);

        await _repository.CreateAsync(lekarstvo);

        return MapToLekarstvoDto(lekarstvo);
    }

    public async Task<LekarstvoDto> UpdateLekarstvoAsync(LekarstvoForUpdateDto lekarstvoForUpdateDto)
    {
        if (lekarstvoForUpdateDto == null)
        {
            throw new ArgumentNullException(nameof(lekarstvoForUpdateDto));
        }

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
            PurchasePrice = lekarstvo.PurchasePrice,
            SalePrice = lekarstvo.SalePrice,
            ExpirationDate = lekarstvo.ExpirationDate,
            BeforeDate = lekarstvo.BeforeDate,
            PhotoBase64 = lekarstvo.PhotoBase64,
            MeasurementUnit = lekarstvo.MeasurementUnit,
            CategoryLekarstvoId = lekarstvo.CategoryLekarstvoId,
            ScladId = lekarstvo.ScladId
        };
    }

    private async Task<Lekarstvo> MapToLekarstvo(LekarstvoForCreateDto lekarstvo)
    {
        return new Lekarstvo
        {
            Name = lekarstvo.Name,
            PurchasePrice = lekarstvo.PurchasePrice,
            SalePrice = lekarstvo.SalePrice,
            ExpirationDate = lekarstvo.ExpirationDate,
            BeforeDate = lekarstvo.BeforeDate,
            PhotoBase64 = lekarstvo.PhotoBase64,
            MeasurementUnit = lekarstvo.MeasurementUnit,
            CategoryLekarstvoId = lekarstvo.CategoryLekarstvoId,
            ScladId = lekarstvo.ScladId
        };
    }

    private LekarstvoDto MapToLekarstvoDto(Lekarstvo lekarstvo)
    {
        return new LekarstvoDto(
            lekarstvo.Id,
            lekarstvo.Name,
            lekarstvo.PurchasePrice,
            lekarstvo.SalePrice,
            lekarstvo.ExpirationDate,
            lekarstvo.BeforeDate,
            lekarstvo.PhotoBase64,
            lekarstvo.MeasurementUnit,
            lekarstvo.CategoryLekarstvoId,
            lekarstvo.CategoryLekarstvo?.Name ?? "",
            lekarstvo.ScladId,
            lekarstvo.Sclad?.Name ?? ""
            );
    }
}
