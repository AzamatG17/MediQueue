using MediQueue.Domain.DTOs.Category;
using MediQueue.Domain.DTOs.Service;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Entities.Responses;
using MediQueue.Domain.Exceptions;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class CategoriesService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IGroupRepository _groupRepository;

    public CategoriesService(ICategoryRepository categoryRepository, IGroupRepository groupRepository)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetCategoriesWithGroupsAsync();

        if (categories == null) return null;

        return categories.Select(MapToCategoryDto).ToList();
    }

    public async Task<CategoryDto> GetCategoryByIdAsync(int id)
    {
        var category = await _categoryRepository.FindByIdWithGroupAsync(id)
            ?? throw new EntityNotFoundException($"Category with {id} not found");

        return MapToCategoryDto(category);
    }

    public async Task<CategoryDto> CreateCategoryAsync(CategoryForCreateDto categoryForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(categoryForCreateDto);

        var category = await MapToCategoryAsync(categoryForCreateDto);

        await _categoryRepository.CreateAsync(category);

        return MapToCategoryDto(category);
    }

    public async Task<CategoryDto> UpdateCategoryAsync(CategoryForUpdateDto categoryForUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(categoryForUpdateDto);

        var category = await _categoryRepository.FindByIdCategoryAsync(categoryForUpdateDto.Id)
            ?? throw new EntityNotFoundException($"Category with {categoryForUpdateDto.Id} not found");
        
        category.CategoryName = categoryForUpdateDto.CategoryName;

        await UpdateGroupsAsync(category, categoryForUpdateDto.GroupIds);

        await _categoryRepository.UpdateAsync(category);

        return MapToCategoryDto(category);
    }

    public async Task DeleteCategoryAsync(int id)
    {
        await _categoryRepository.DeleteAsync(id);
    }

    private async Task<Category> MapToCategoryAsync(CategoryForCreateDto categoryForCreateDto)
    {
        var groups = await _groupRepository.FindByGroupIdsAsync(categoryForCreateDto.GroupIds);

        var missingGroupIds = categoryForCreateDto.GroupIds.Except(groups.Select(g => g.Id)).ToList();

        if (missingGroupIds.Any())
        {
            throw new ArgumentException($"The following group IDs were not found: {string.Join(", ", missingGroupIds)}");
        }

        return new Category
        {
            CategoryName = categoryForCreateDto.CategoryName,
            Groups = groups.ToList()
        };
    }

    private static CategoryDto MapToCategoryDto(Category category)
    {
        var groupInfos = category.Groups.Select(g => new GroupInfoResponse(
            g.Id,
            g.GroupName
            )).ToList();

        var serviceDtos = category.Services?.Select(s => new ServiceHelperDto(
            s.Id,
            s.Name,
            s.Amount,
            s.CategoryId ?? 0,
            s.Category?.CategoryName ?? ""
            )).ToList();

        return new CategoryDto(
            category.Id,
            category.CategoryName,
            groupInfos,
            serviceDtos
        );
    }

    private async Task UpdateGroupsAsync(Category category, List<int> newGroupIds)
    {
        // Get existing group IDs
        var existingGroups = category.Groups.ToList();
        var existingGroupIds = existingGroups.Select(g => g.Id).ToHashSet();

        // Retrieve updated groups from the repository
        var updatedGroups = await _groupRepository.FindByGroupIdsAsync(newGroupIds);
        var updatedGroupIds = updatedGroups.Select(g => g.Id).ToHashSet();

        // Determine which groups to add and which to remove
        var groupsToAdd = updatedGroups.Where(g => !existingGroupIds.Contains(g.Id)).ToList();
        var groupsToRemove = existingGroups.Where(g => !updatedGroupIds.Contains(g.Id)).ToList();

        // Remove groups that are no longer associated with the category
        foreach (var groupToRemove in groupsToRemove)
        {
            category.Groups.Remove(groupToRemove);
        }

        // Add new groups to the category
        foreach (var groupToAdd in groupsToAdd)
        {
            category.Groups.Add(groupToAdd);
        }
    }
}
