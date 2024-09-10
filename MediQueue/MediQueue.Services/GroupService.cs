using AutoMapper;
using MediQueue.Domain.DTOs.Category;
using MediQueue.Domain.DTOs.Group;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Entities.Responses;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GroupService(IGroupRepository groupRepository, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task<IEnumerable<GroupDto>> GetAllGroupsAsync()
        {
            var group = await _groupRepository.GetGroupWithGroupsAsync();

            return group.Select(MapToCategoryDto).ToList();
        }

        public async Task<GroupDto> GetGroupByIdAsync(int id)
        {
            var group = await _groupRepository.FindByIdWithGroupAsync(id);
            if (group == null)
            {
                throw new KeyNotFoundException($"Group with {id} not found");
            }

            return MapToCategoryDto(group);
        }

        public async Task<GroupDto> CreateGroupAsync(GroupForCreateDto groupForCreateDto)
        {
            if (groupForCreateDto == null)
            {
                throw new ArgumentNullException(nameof(groupForCreateDto));
            }

            var group = await MapToCategoryAsync(groupForCreateDto);

            await _groupRepository.CreateAsync(group);

            return MapToCategoryDto(group);
        }

        public async Task<GroupDto> UpdateGroupAsync(GroupForUpdateDto groupForUpdateDto)
        {
            if (groupForUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(groupForUpdateDto));
            }

            var category = await _groupRepository.FindByIdAsync(groupForUpdateDto.Id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with {groupForUpdateDto.Id} not found");
            }

            category.GroupName = groupForUpdateDto.GroupName;

            var groups = await _categoryRepository.FindByGroupIdsAsync(groupForUpdateDto.CategoryIds);
            category.Categories = groups.ToList();

            await _groupRepository.UpdateAsync(category);

            return MapToCategoryDto(category);
        }

        public async Task DeleteGroupAsync(int id)
        {
            await _groupRepository.DeleteAsync(id); 
        }

        private async Task<Group> MapToCategoryAsync(GroupForCreateDto groupForCreateDto)
        {
            var groups = await _categoryRepository.FindByGroupIdsAsync(groupForCreateDto.CategoryIds);
            return new Group
            {
                GroupName = groupForCreateDto.GroupName,
                Categories = groups.ToList()
            };
        }

        private GroupDto MapToCategoryDto(Group category)
        {
            var groupInfos = category.Categories.Select(g => new GroupInfoResponse
            {
                Id = g.Id,
                Name = g.CategoryName,
            }).ToList();

            return new GroupDto(
                category.Id,
                category.GroupName,
                groupInfos
            );
        }
    }
}
