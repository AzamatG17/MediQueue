using MediQueue.Domain.DTOs.Lekarstvo;
using MediQueue.Domain.DTOs.Sclad;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class ScladService : IScladService
{
    private readonly IScladRepository _cladRepository;
    private readonly IBranchRepository _branchRepository;

    public ScladService(IScladRepository cladRepository, IBranchRepository branchRepository)
    {
        _cladRepository = cladRepository ?? throw new ArgumentNullException(nameof(cladRepository));
        _branchRepository = branchRepository ?? throw new ArgumentNullException( nameof(branchRepository));
    }

    public async Task<IEnumerable<ScladDto>> GetAllScladsAsync()
    {
        var sclads = await _cladRepository.FindAllScladAsync();

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
        var sclad = await _cladRepository.FindbyIdScladAsync(id);

        return await MapToScladDto(sclad);
    }

    public async Task<ScladDto> CreateScladAsync(ScladForCreateDto scladForCreateDto)
    {
        if (scladForCreateDto == null)
        {
            throw new ArgumentNullException(nameof(scladForCreateDto));
        }

        var sclad = await MapToScladForCreate(scladForCreateDto);

        await _cladRepository.CreateAsync(sclad);

        return await MapToScladDto(sclad);
    }

    public async Task<ScladDto> UpdateScladAsync(ScladForUpdateDto scladForUpdateDto)
    {
        if (scladForUpdateDto == null)
        {
            throw new ArgumentNullException(nameof(scladForUpdateDto));
        }

        var sclad = await MapToScladForUpdate(scladForUpdateDto);

        await _cladRepository.UpdateAsync(sclad);

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
            sclad.Lekarstvos != null
                ? sclad.Lekarstvos.Select(MapToLekarstvoDto).ToList()
                : new List<LekarstvoDto>());
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
            lekarstvo.CategoryLekarstvo.Name,
            lekarstvo.ScladId,
            lekarstvo.Sclad.Name);
    }
}
