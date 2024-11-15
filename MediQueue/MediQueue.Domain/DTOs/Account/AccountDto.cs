using MediQueue.Domain.DTOs.Role;
using MediQueue.Domain.DTOs.Service;

namespace MediQueue.Domain.DTOs.Account;

public record AccountDto(
    int Id,
    string Login,
    string Password,
    string Passport,
    string PhoneNumber, 
    string FirstName, 
    string LastName,
    string SurName,
    string PhotoBase64,
    DateTime Bithdate,
    int RoleId, 
    string RoleName,
    int? CabinetId,
    string? CabinetNumber,
    List<RolePermissionDto>? RolePermissions,
    List<ServiceHelperDto>? ServiceDtos);
