using AutoMapper;
using MediQueue.Domain.DTOs.Group;
using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Mappings
{
    public class GroupMappings : Profile
    {
        public GroupMappings()
        {
            CreateMap<GroupDto, Group>();
            CreateMap<Group, GroupDto>();
            CreateMap<GroupForCreateDto, Group>();
            CreateMap<GroupForUpdateDto, Group>();
        }
    }
}
