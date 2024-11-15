namespace MediQueue.Domain.DTOs.Account;

public record AccountHelperDto(
    int Id,
    string PhoneNumber,
    string AccountName,
    string PhotoBase64,
    DateTime Bithdate,
    int RoleId,
    string RoleName,
    int? CabinetId,
    string? CabinetNumber);    
