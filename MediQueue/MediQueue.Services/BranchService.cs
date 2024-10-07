using AutoMapper;
using MediQueue.Domain.DTOs.Branch;
using MediQueue.Domain.DTOs.Lekarstvo;
using MediQueue.Domain.DTOs.Sclad;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class BranchService : IBranchService
{
    private readonly IBranchRepository _repository;
    private readonly IMapper _mapper;
    public BranchService(IBranchRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<BranchDto>> GetAllBranchsAsync()
    {
        var branches = await _repository.FindAllBranches();

        return branches.Select(MapToBranchDto).ToList();
    }

    public async Task<BranchDto> GetBranchByIdAsync(int id)
    {
        var branch = await _repository.FindByIdBranch(id);

        return MapToBranchDto(branch);
    }

    public async Task<BranchDto> CreateBranchAsync(BranchForCreateDto branchForCreateDto)
    {
        if (branchForCreateDto == null)
        {
            throw new ArgumentNullException(nameof(branchForCreateDto));
        }

        var branch = await MapToBranch(branchForCreateDto);

        await _repository.CreateAsync(branch);

        return MapToBranchDto(branch);
    }

    public async Task<BranchDto> UpdateBranchAsync(BranchForUpdatreDto branchForUpdatreDto)
    {
        if (branchForUpdatreDto == null)
        {
            throw new ArgumentNullException(nameof(branchForUpdatreDto));
        }

        var branch = await MapToBranchForUpdate(branchForUpdatreDto);

        await _repository.UpdateAsync(branch);

        return MapToBranchDto(branch);
    }

    public async Task DeleteBranchAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private async Task<Branch> MapToBranch(BranchForCreateDto branchForCreateDto)
    {
        return new Branch
        {
            Name = branchForCreateDto.Name ?? null,
            Addres = branchForCreateDto.Addres ?? null,
        };
    }

    private async Task<Branch> MapToBranchForUpdate(BranchForUpdatreDto branchForUpdatreDto)
    {
        return new Branch
        {
            Id = branchForUpdatreDto.Id,
            Name = branchForUpdatreDto.Name ?? null,
            Addres = branchForUpdatreDto.Addres ?? null,
        };
    }

    private BranchDto MapToBranchDto(Branch branch)
    {
        return new BranchDto(
            branch.Id,
            branch.Name,
            branch.Addres,
            branch.Sclads != null
                ? branch.Sclads.Select(MapToScladDto).ToList()
                : new List<ScladDto>());
    }

    private ScladDto MapToScladDto(Sclad? sclad)
    {
        return new ScladDto(
            sclad.Id,
            sclad.Name,
            sclad.Branchid,
            sclad.Branch.Name,
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
            lekarstvo.TotalQuantity,
            lekarstvo.PriceQuantity,
            lekarstvo.MeasurementUnit,
            lekarstvo.CategoryLekarstvoId,
            lekarstvo.CategoryLekarstvo.Name,
            lekarstvo.ScladId,
            lekarstvo.Sclad.Name
            );
    }
}
