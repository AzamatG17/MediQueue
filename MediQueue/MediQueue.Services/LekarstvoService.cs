﻿using MediQueue.Domain.DTOs.Lekarstvo;
using MediQueue.Domain.DTOs.Partiya;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Exceptions;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Domain.ResourceParameters;

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

    public async Task<IEnumerable<LekarstvoDto>> GetAllLekarstvosAsync(LekarstvoResourceParametrs lekarstvoResourceParametrs)
    {
        var lekarstvo = await _repository.FindAllLekarstvoAsync(lekarstvoResourceParametrs);

        if (lekarstvo == null) return null;

        return lekarstvo.Select(MapToLekarstvoDto).ToList();
    }

    public async Task<LekarstvoDto> GetLekarstvoByIdAsync(int id)
    {
        var lekarstvo = await _repository.FindByIdLekarstvoAsync(id)
            ?? throw new EntityNotFoundException($"Lekarstvo with id: {id} does not exist.");

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

        var lekarstvo = await _repository.FindByIdAsync(lekarstvoForUpdateDto.Id)
            ?? throw new EntityNotFoundException($"Lekarstov with id: {lekarstvoForUpdateDto.Id} does not exist.");

        lekarstvo.Name = lekarstvoForUpdateDto.Name;
        lekarstvo.PhotoBase64 = lekarstvoForUpdateDto.PhotoBase64;
        lekarstvo.MeasurementUnit = lekarstvoForUpdateDto.MeasurementUnit;
        lekarstvo.CategoryLekarstvoId = lekarstvoForUpdateDto.CategoryLekarstvoId;

        await _repository.UpdateAsync(lekarstvo);

        return MapToLekarstvoDto(lekarstvo);
    }

    public async Task DeleteLekarstvoAsync(int id)
    {
        await _repository.DeleteAsync(id);
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

    private static LekarstvoDto MapToLekarstvoDto(Lekarstvo lekarstvo)
    {
        decimal totalQuantityLEkarstvo = lekarstvo.Partiyas?.Sum(x => x.TotalQuantity ?? 0) ?? 0;

        return new LekarstvoDto(
            lekarstvo.Id,
            lekarstvo.Name,
            lekarstvo.PhotoBase64,
            lekarstvo.MeasurementUnit.ToString(),
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
                lekarstvo.MeasurementUnit.ToString(),
                p.LekarstvoId,
                p.Lekarstvo?.Name ?? "",
                p.ScladId,
                p.Sclad?.Name ?? ""
            )).ToList() ?? new List<PartiyaDto>()
        );
    }
}
