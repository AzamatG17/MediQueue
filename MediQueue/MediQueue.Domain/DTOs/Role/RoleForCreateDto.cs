namespace MediQueue.Domain.DTOs.Role
{
    public record RoleForCreateDto(string Name, List<int> PermissionId);
}
