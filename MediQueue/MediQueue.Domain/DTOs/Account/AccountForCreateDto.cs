﻿using MediQueue.Domain.DTOs.Role;

namespace MediQueue.Domain.DTOs.Account;

public record AccountForCreateDto(
    string Login,
    string Password,
    string Passport,
    string PhoneNumber,
    string FirstName,
    string LastName, 
    string SurName, 
    string? PhotoBase64,
    DateTime Bithdate, 
    int RoleId, 
    string? RoomNumber,
    List<RolePermissionDto>? RolePermissions,
    List<int>? ServiceIds
    );
