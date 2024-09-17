using MediQueue.Domain.DTOs.Branch;

namespace MediQueue.Domain.Interfaces.Services;

public interface IBranchService
{
    Task<IEnumerable<BranchDto>> GetAllBranchsAsync();
    Task<BranchDto> GetBranchByIdAsync(int id);
    Task<BranchDto> CreateBranchAsync(BranchForCreateDto branchForCreateDto);
    Task<BranchDto> UpdateBranchAsync(BranchForUpdatreDto branchForUpdatreDto);
    Task DeleteBranchAsync(int id);
}
