using MediQueue.Domain.DTOs.Permission;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync();
        Task<PermissionDto> GetPermissionByIdAsync(int id);
        Task<PermissionDto> CreatePermissionAsync(PermissionForCreateDto permissionForCreateDto);
        Task<PermissionDto> UpdatePermissionAsync(PermissionForUpdateDto permissionForUpdateDto);
        Task DeletePermissionAsync(int id);
    }
}
