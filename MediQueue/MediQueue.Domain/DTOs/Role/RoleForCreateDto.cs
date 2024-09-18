using MediQueue.Domain.Entities;

namespace MediQueue.Domain.DTOs.Role
{
    public record RoleForCreateDto(string Name, List<RolePermissionDto> PermissionId);
}
