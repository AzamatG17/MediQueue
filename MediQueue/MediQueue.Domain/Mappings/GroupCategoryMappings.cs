using AutoMapper;
using MediQueue.Domain.DTOs.GroupCategory;
using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Mappings
{
    public class GroupCategoryMappings : Profile
    {
        public GroupCategoryMappings()
        {
            CreateMap<GroupCategoryDto, GroupCategory>();
            CreateMap<GroupCategory, GroupCategoryDto>();
            CreateMap<GroupCategoryForCreate, GroupCategory>();
            CreateMap<GroupCategoryForUpdate, GroupCategory>();
        }
    }
}
