using MediQueue.Domain.DTOs.Branch;
using MediQueue.Domain.DTOs.Partiya;
using MediQueue.Domain.DTOs.Sclad;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class BranchService : IBranchService
{
    private readonly IBranchRepository _repository;

    public BranchService(IBranchRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IEnumerable<BranchDto>> GetAllBranchsAsync()
    {
        var branches = await _repository.FindAllBranches();

        if (branches == null) return null;

        return branches.Select(MapToBranchDto).ToList();
    }

    public async Task<BranchDto> GetBranchByIdAsync(int id)
    {
        var branch = await _repository.FindByIdBranch(id)
            ?? throw new KeyNotFoundException($"Branch with {id} not found");

        return MapToBranchDto(branch);
    }

    public async Task<BranchDto> CreateBranchAsync(BranchForCreateDto branchForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(branchForCreateDto);

        var branch = MapToBranch(branchForCreateDto);

        await _repository.CreateAsync(branch);

        return MapToBranchDto(branch);
    }

    public async Task<BranchDto> UpdateBranchAsync(BranchForUpdatreDto branchForUpdatreDto)
    {
        ArgumentNullException.ThrowIfNull(branchForUpdatreDto);

        var branch = MapToBranchForUpdate(branchForUpdatreDto);

        await _repository.UpdateAsync(branch);

        return MapToBranchDto(branch);
    }

    public async Task DeleteBranchAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private static Branch MapToBranch(BranchForCreateDto branchForCreateDto)
    {
        return new Branch
        {
            Name = branchForCreateDto.Name ?? null,
            Addres = branchForCreateDto.Addres ?? null,
        };
    }

    private static Branch MapToBranchForUpdate(BranchForUpdatreDto branchForUpdatreDto)
    {
        return new Branch
        {
            Id = branchForUpdatreDto.Id,
            Name = branchForUpdatreDto.Name ?? null,
            Addres = branchForUpdatreDto.Addres ?? null,
        };
    }

    private static BranchDto MapToBranchDto(Branch branch)
    {
        return new BranchDto(
            branch.Id,
            branch.Name,
            branch.Addres,
            branch.Sclads != null
                ? branch.Sclads.Select(MapToScladDto).ToList()
                : new List<ScladDto>());
    }

    private static ScladDto MapToScladDto(Sclad? sclad)
    {
        return new ScladDto(
            sclad.Id,
            sclad.Name,
            sclad.Branchid,
            sclad.Branch?.Name ?? "",
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
            p.Lekarstvo?.MeasurementUnit.ToString(),
            p.LekarstvoId,
            p.Lekarstvo?.Name ?? "",
            p.ScladId,
            p.Sclad?.Name ?? "");
    }
}
