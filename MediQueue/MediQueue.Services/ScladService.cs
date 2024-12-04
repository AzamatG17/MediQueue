using MediQueue.Domain.DTOs.Partiya;
using MediQueue.Domain.DTOs.Sclad;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.Persistence;

namespace MediQueue.Services;

public class ScladService : IScladService
{
    private readonly IScladRepository _cladRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly MediQueueDbContext _context;

    public ScladService(IScladRepository cladRepository, IBranchRepository branchRepository, MediQueueDbContext mediQueueDbContext)
    {
        _cladRepository = cladRepository ?? throw new ArgumentNullException(nameof(cladRepository));
        _branchRepository = branchRepository ?? throw new ArgumentNullException(nameof(branchRepository));
        _context = mediQueueDbContext ?? throw new ArgumentNullException(nameof(mediQueueDbContext));
    }

    public async Task<IEnumerable<ScladDto>> GetAllScladsAsync()
    {
        var sclads = await _cladRepository.FindAllScladAsync();

        if (sclads == null) return null;

        var scladDtos = new List<ScladDto>();

        foreach (var sclad in sclads)
        {
            var scladDto = await MapToScladDto(sclad);
            scladDtos.Add(scladDto);
        }

        return scladDtos.ToList();
    }

    public async Task<ScladDto> GetScladByIdAsync(int id)
    {
        var sclad = await _cladRepository.FindbyIdScladAsync(id)
            ?? throw new KeyNotFoundException($"Sclad with id: {id} does not exist.");

        return await MapToScladDto(sclad);
    }

    public async Task<ScladDto> CreateScladAsync(ScladForCreateDto scladForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(scladForCreateDto);

        var branchExists = await _branchRepository.ExistsAsync(scladForCreateDto.Branchid);
        if (!branchExists)
        {
            throw new InvalidOperationException($"Branch with ID {scladForCreateDto.Branchid} does not exist.");
        }

        var sclad = await MapToScladForCreate(scladForCreateDto);

        await _cladRepository.CreateAsync(sclad);

        return await MapToScladDto(sclad);
    }

    public async Task<ScladDto> UpdateScladAsync(ScladForUpdateDto scladForUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(scladForUpdateDto);

        var existingSclad = await _cladRepository.FindByIdScladAsync(scladForUpdateDto.Id);
        if (existingSclad == null)
        {
            throw new InvalidOperationException($"Sclad with ID {scladForUpdateDto.Id} does not exist.");
        }

        existingSclad.Name = scladForUpdateDto.Name;
        existingSclad.Branchid = scladForUpdateDto.Branchid;

        var sclad = await MapToScladForUpdate(scladForUpdateDto);

        await _context.SaveChangesAsync();

        return await MapToScladDto(sclad);
    }

    public async Task DeleteScladAsync(int id)
    {
        await _cladRepository.DeleteAsync(id);
    }

    private async Task<Sclad> MapToScladForUpdate(ScladForUpdateDto scladForUpdateDto)
    {
        return new Sclad
        {
            Id = scladForUpdateDto.Id,
            Name = scladForUpdateDto.Name,
            Branchid = scladForUpdateDto.Branchid
        };
    }

    private async Task<Sclad> MapToScladForCreate(ScladForCreateDto scladForCreateDto)
    {
        return new Sclad
        {
            Name = scladForCreateDto.Name,
            Branchid = scladForCreateDto.Branchid
        };
    }

    private async Task<ScladDto> MapToScladDto(Sclad sclad)
    {
        var branch = await _branchRepository.FindByIdAsync(sclad.Branchid);

        return new ScladDto(
            sclad.Id,
            sclad.Name,
            sclad.Branchid,
            branch.Name,
            sclad.Partiyas != null
                ? sclad.Partiyas.Select(MapToLekarstvoDto).ToList()
                : new List<PartiyaDto>());
    }

    private static PartiyaDto MapToLekarstvoDto(Partiya p)
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
