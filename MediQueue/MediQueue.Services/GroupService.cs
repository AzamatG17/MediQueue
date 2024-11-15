using AutoMapper;
using MediQueue.Domain.DTOs.Category;
using MediQueue.Domain.DTOs.Group;
using MediQueue.Domain.DTOs.Service;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Entities.Responses;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

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

    public async Task<IEnumerable<GroupForAllDateDto>> GetAllGroupsAsync()
    {
        var group = await _groupRepository.GetGroupWithGroupsAsync();

        return group.Select(MapToGroupAllDateDto).ToList();
    }

    public async Task<GroupForAllDateDto> GetGroupByIdAsync(int id)
    {
        var group = await _groupRepository.FindByIdWithGroupAsync(id);
        if (group == null)
        {
            throw new KeyNotFoundException($"Group with {id} not found");
        }

        return MapToGroupAllDateDto(group);
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
        ArgumentNullException.ThrowIfNull(groupForUpdateDto);

        var group = await _groupRepository.FindByIdWithGroupAsync(groupForUpdateDto.Id);
        if (group == null)
        {
            throw new KeyNotFoundException($"Group with {groupForUpdateDto.Id} not found");
        }

        group.GroupName = groupForUpdateDto.GroupName;

        var existingCategoryIds = group.Categories.Select(c => c.Id).ToList();

        var updatedCategories = await _categoryRepository.FindByGroupIdsAsync(groupForUpdateDto.CategoryIds);
        var updatedCategoryIds = updatedCategories.Select(c => c.Id).ToList();

        var categoriesToAdd = updatedCategories.Where(c => !existingCategoryIds.Contains(c.Id)).ToList();

        var categoriesToRemove = group.Categories.Where(c => !updatedCategoryIds.Contains(c.Id)).ToList();

        foreach (var categoryToRemove in categoriesToRemove)
        {
            group.Categories.Remove(categoryToRemove);
        }

        foreach (var categoryToAdd in categoriesToAdd)
        {
            group.Categories.Add(categoryToAdd);
        }

        await _groupRepository.UpdateAsync(group);

        return MapToCategoryDto(group);
    }

    public async Task DeleteGroupAsync(int id)
    {
        await _groupRepository.DeleteGroupAsync(id);
    }

    private async Task<Group> MapToCategoryAsync(GroupForCreateDto groupForCreateDto)
    {
        return new Group
        {
            GroupName = groupForCreateDto.GroupName
        };
    }

    private GroupDto MapToCategoryDto(Group category)
    {
        var groupInfos = category.Categories?.Select(g => new GroupInfoResponse(
            g.Id,
            g.CategoryName
            )).ToList();

        return new GroupDto(
            category.Id,
            category.GroupName,
            groupInfos
        );
    }

    private GroupForAllDateDto MapToGroupAllDateDto(Group group)
    {
        return new GroupForAllDateDto(
            group.Id,
            group.GroupName,
            group.Categories.Select(MapToCategoryDto).ToList()
        );
    }

    private CategoryForGroupDto MapToCategoryDto(Category category)
    {
        return new CategoryForGroupDto(
            category.Id,
            category.CategoryName,
            category.Services.Select(MapToServiceDto).ToList()
        );
    }

    private ServiceHelperDto MapToServiceDto(Service service)
    {
        return new ServiceHelperDto(
            service.Id,
            service.Name,
            service.Amount,
            service.CategoryId ?? 0,
            service.Category?.CategoryName ?? ""
        );
    }
}
