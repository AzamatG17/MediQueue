using MediQueue.Domain.DTOs.Partiya;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class PartiyaService : IPartiyaService
{
    private readonly IPartiyaRepository _repository;
    private readonly ILekarstvoRepository _lekarstvoRepository;
    private readonly IScladRepository _cladRepository;

    public PartiyaService(IPartiyaRepository repository, ILekarstvoRepository lekarstvoRepository, IScladRepository cladRepository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _lekarstvoRepository = lekarstvoRepository ?? throw new ArgumentNullException(nameof(lekarstvoRepository));
        _cladRepository = cladRepository ?? throw new ArgumentNullException(nameof(cladRepository));
    }

    public async Task<IEnumerable<PartiyaDto>> GetAllPartiyastvosAsync()
    {
        var partiyas = await _repository.FindAllPartiyaAsync();

        if (partiyas == null) return null;

        return partiyas.Select(MapToPartiyaDto).ToList();
    }

    public async Task<PartiyaDto> GetPartiyaByIdAsync(int id)
    {
        var partiya = await _repository.FindByIdPartiyaAsync(id)
            ?? throw new KeyNotFoundException($"Partiya with id: {id} does not exist.");

        return MapToPartiyaDto(partiya);
    }

    public async Task<PartiyaDto> CreatePartiyaAsync(PartiyaForCreateDto partiyaForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(partiyaForCreateDto));

        if (!await _lekarstvoRepository.IsExistByIdAsync(partiyaForCreateDto.LekarstvoId))
        {
            throw new ArgumentException($"Lekarstvo with id: {partiyaForCreateDto.LekarstvoId} does not exist");
        }

        if (!await _cladRepository.IsExistByIdAsync(partiyaForCreateDto.ScladId))
        {
            throw new ArgumentException($"Sclad with id: {partiyaForCreateDto.ScladId} does not exist");
        }

        var partiya = new Partiya
        {
            PurchasePrice = partiyaForCreateDto.PurchasePrice,
            SalePrice = partiyaForCreateDto.SalePrice,
            ExpirationDate = partiyaForCreateDto.ExpirationDate,
            BeforeDate = partiyaForCreateDto.BeforeDate,
            TotalQuantity = partiyaForCreateDto.TotalQuantity,
            PriceQuantity = partiyaForCreateDto.PriceQuantity,
            PhotoBase64 = partiyaForCreateDto?.PhotoBase64,
            LekarstvoId = partiyaForCreateDto?.LekarstvoId,
            ScladId = partiyaForCreateDto?.ScladId
        };

        await _repository.CreateAsync(partiya);

        return MapToPartiyaDto(partiya);
    }

    public async Task<PartiyaDto> UpdatePartiyaAsync(PartiyaForUpdateDto partiyaForUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(partiyaForUpdateDto));

        var partiya = await _repository.FindByIdPartiyAsync(partiyaForUpdateDto.Id);

        if (partiya == null) return null;

        if (!await _lekarstvoRepository.IsExistByIdAsync(partiyaForUpdateDto.LekarstvoId))
        {
            throw new ArgumentException($"Lekarstvo with id: {partiyaForUpdateDto.LekarstvoId} does not exist");
        }

        if (!await _cladRepository.IsExistByIdAsync(partiyaForUpdateDto.ScladId))
        {
            throw new ArgumentException($"Sclad with id: {partiyaForUpdateDto.ScladId} does not exist");
        }

        partiya.PurchasePrice = partiyaForUpdateDto.PurchasePrice;
        partiya.SalePrice = partiyaForUpdateDto.SalePrice;
        partiya.ExpirationDate = partiyaForUpdateDto.ExpirationDate;
        partiya.BeforeDate = partiyaForUpdateDto.BeforeDate;
        partiya.TotalQuantity = partiyaForUpdateDto.TotalQuantity;
        partiya.PriceQuantity = partiyaForUpdateDto.PriceQuantity;
        partiya.PhotoBase64 = partiyaForUpdateDto?.PhotoBase64 ?? "";
        partiya.LekarstvoId = partiyaForUpdateDto?.LekarstvoId;
        partiya.ScladId = partiyaForUpdateDto?.ScladId;

        await _repository.UpdateAsync(partiya);

        return MapToPartiyaDto(partiya);
    }

    public async Task DeletePartiyaAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private static PartiyaDto MapToPartiyaDto(Partiya p)
    {
        return new PartiyaDto(
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
            p.Sclad?.Name ?? "");
    }
}
