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
            //CreateMap<Role, RoleDto>()
            //.ForMember(dest => dest.PermissionId, opt => opt.MapFrom(src =>
            //    src.RolePermissions != null ? src.RolePermissions.Select(rp => rp.PermissionId).ToList() : new List<int>()));

            ////CreateMap<RoleForCreateDto, Role>();
            //CreateMap<RoleForCreateDto, Role>()
            //.ForMember(dest => dest.RolePermissions, opt => opt.MapFrom(src =>
            //    src.PermissionId != null ? src.PermissionId.Select(pId => new RolePermission { PermissionId = pId }).ToList() : new List<RolePermission>()));
            CreateMap<RoleForUpdateDto, Role>();
            
        }
    }
}
