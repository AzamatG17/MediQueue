namespace MediQueue.Domain.DTOs.Role;

public record RolePermissionDto(int ControllerId, ICollection<int> Permissions);
