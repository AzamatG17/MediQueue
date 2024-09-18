using MediQueue.Domain.DTOs.Role;

namespace MediQueue.Domain.DTOs.Account
{
    public record AccountDto(
        int Id, string Login, string Password, string Passport, string PhoneNumber, string FirstName, string LastName, string SurName, string Email, DateTime Bithdate, int RoleId, string RoleName, List<RolePermissionDto>? RolePermissions);
}
