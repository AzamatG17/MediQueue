using MediQueue.Domain.DTOs.Role;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto> GetRoleByIdAsync(int id);
        Task<RoleDto> CreateRoleAsync(RoleForCreateDto roleForCreateDto);
        Task<RoleDto> UpdateRoleAsync(RoleForUpdateDto roleForUpdateDto);
        Task DeleteRoleAsync(int id);
    }
}
