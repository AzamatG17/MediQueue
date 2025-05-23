﻿using AutoMapper;
using MediQueue.Domain.DTOs.Account;
using MediQueue.Domain.DTOs.Role;
using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Mappings
{
    public class AccountMappings : Profile
    {
        public AccountMappings()
        {
            CreateMap<AccountDto, Account>();
            CreateMap<Account, AccountDto>()
                .ForMember(x => x.RoleName, e => e.MapFrom(d => d.Role.Name));
            CreateMap<AccountForCreateDto, Account>();
            CreateMap<AccountForUpdateDto, Account>()
                .ForMember(dest => dest.RolePermissions, opt => opt.MapFrom(src => src.RolePermissions));

            CreateMap<RolePermissionDto, RolePermission>();
        }
    }
}
