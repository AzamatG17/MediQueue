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
            CreateMap<Role, RoleDto>();
            CreateMap<RoleForCreateDto, Role>();
            CreateMap<RoleForUpdateDto, Role>();
        }
    }
}
