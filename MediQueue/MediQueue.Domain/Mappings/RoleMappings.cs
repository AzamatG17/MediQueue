using AutoMapper;
using MediQueue.Domain.DTOs.Role;
using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Mappings
{
    public class RoleMappings : Profile
    {
        public RoleMappings()
        {
            CreateMap<RoleDto, Role>();

            CreateMap<Role, RoleDto>()
            .ForMember(dest => dest.PermissionId, opt => opt.MapFrom(src => src.RolePermissions.Select(rp => rp.PermissionId).ToList()));

            CreateMap<RoleForCreateDto, Role>()
                .ForMember(dest => dest.RolePermissions, opt => opt.MapFrom(src => src.PermissionId.Select(id => new RolePermission { PermissionId = id }).ToList()));

            CreateMap<RoleForUpdateDto, Role>()
                .ForMember(dest => dest.RolePermissions, opt => opt.MapFrom(src => src.PermissionId.Select(id => new RolePermission { PermissionId = id }).ToList()));
        }
    }
}
