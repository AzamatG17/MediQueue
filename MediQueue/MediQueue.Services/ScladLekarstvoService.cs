using MediQueue.Domain.DTOs.ScladLekarstvo;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class ScladLekarstvoService : IScladLekarstvoService
{
    private readonly IScladLekarstvoRepository _repository;
    private readonly IScladRepository _cladRepository;
    private readonly IPartiyaRepository _partiyaRepository;

    public ScladLekarstvoService(
        IScladLekarstvoRepository scladLekarstvoRepository, 
        IScladRepository scladRepository,
        IPartiyaRepository partiyaRepository)
    {
        _repository = scladLekarstvoRepository ?? throw new ArgumentNullException(nameof(scladLekarstvoRepository));
        _cladRepository = scladRepository ?? throw new ArgumentNullException(nameof(scladRepository));
        _partiyaRepository = partiyaRepository ?? throw new ArgumentNullException(nameof(partiyaRepository));
    }

    public async Task<IEnumerable<ScladLekarstvoDto>> GetAllScladLekarstvosAsync()
    {
        var scladLekarstvos = await _repository.FindAllScladLekarstvoAsync();

        if (scladLekarstvos == null) return null;

        return scladLekarstvos.Select(MapToScladLekarstvoDto).ToList();
    }

    public async Task<ScladLekarstvoDto> GetScladLekarstvoByIdAsync(int id)
    {
        var scladLekarstvo = await _repository.FindByIdScladLekarstvoAsync(id);

        if (scladLekarstvo == null) return null;

        return MapToScladLekarstvoDto(scladLekarstvo); 
    }

    public async Task<ScladLekarstvoDto> CreateScladLekarstvoAsync(ScladLekarstvoForCreate scladLekarstvoForCreate)
    {
        ArgumentNullException.ThrowIfNull(nameof(scladLekarstvoForCreate));

        if (!await _cladRepository.IsExistByIdAsync(scladLekarstvoForCreate.ScladId))
        {
            throw new ArgumentException($"Sclad with id: {scladLekarstvoForCreate.ScladId} does not exist");
        }

        if (!await _partiyaRepository.IsExistByIdAsync(scladLekarstvoForCreate.PartiyaId))
        {
            throw new ArgumentException($"Partiya with id: {scladLekarstvoForCreate.ScladId} does not exist");
        }

        var scladLekarstvo = new ScladLekarstvo
        {
            Quantity = scladLekarstvoForCreate.Quantity,
            ScladId = scladLekarstvoForCreate?.ScladId,
            PartiyaId = scladLekarstvoForCreate?.PartiyaId
        };

        await _repository.CreateAsync(scladLekarstvo);

        return MapToScladLekarstvoDto(scladLekarstvo);
    }

    public Task<ScladLekarstvoDto> UpdateScladLekarstvoAsync(ScladLekarstvoForUpdate scladLekarstvoForUpdate)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteScladLekarstvoAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private ScladLekarstvoDto MapToScladLekarstvoDto(ScladLekarstvo lekarstvo)
    {
        return new ScladLekarstvoDto(
            lekarstvo.Id,
            lekarstvo.Quantity,
            lekarstvo.ScladId,
            lekarstvo.Sclad?.Name ?? "",
            lekarstvo.PartiyaId,
            lekarstvo.Partiya?.Lekarstvo?.Name ?? ""
            );
    }

    private async Task<ScladLekarstvo> MapToScladLekarstvo(ScladLekarstvoDto scladLekarstvoDto)
    {
        return new ScladLekarstvo
        {
             Quantity = scladLekarstvoDto.Quantity,
             ScladId = scladLekarstvoDto?.ScladId,
             PartiyaId = scladLekarstvoDto?.PartiyaId
        };
    }
}
