using AutoMapper;
using MediQueue.Domain.DTOs.Permission;
using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Mappings
{
    public class PermissionMappings : Profile
    {
        public PermissionMappings()
        {
            CreateMap<PermissionDto, Permission>();
            CreateMap<Permission, PermissionDto>();
            CreateMap<PermissionForCreateDto, Permission>();
            CreateMap<PermissionForUpdateDto, Permission>();
        }
    }
}
