using MediQueue.Domain.DTOs.Role;

namespace MediQueue.Domain.DTOs.Account;

public record AccountForUpdateDto(
    int Id, string Login, string Password, string Passport, string PhoneNumber, string FirstName, string LastName, string SurName, string Email, DateTime Birthdate, int RoleId, List<RolePermissionDto>? RolePermissions);
