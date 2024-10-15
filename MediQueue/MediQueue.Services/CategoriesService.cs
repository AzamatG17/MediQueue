using AutoMapper;
using MediQueue.Domain.DTOs.Category;
using MediQueue.Domain.DTOs.Service;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Entities.Responses;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class CategoriesService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IMapper _mapper;

    public CategoriesService(ICategoryRepository categoryRepository, IMapper mapper, IGroupRepository groupRepository)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
    }
    
    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetCategoriesWithGroupsAsync();

        return categories.Select(MapToCategoryDto).ToList();
    }

    public async Task<CategoryDto> GetCategoryByIdAsync(int id)
    {
        var category = await _categoryRepository.FindByIdWithGroupAsync(id);
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with {id} not found");
        }

        return MapToCategoryDto(category);
    }

    public async Task<CategoryDto> CreateCategoryAsync(CategoryForCreateDto categoryForCreateDto)
    {
        if (categoryForCreateDto == null)
        {
            throw new ArgumentNullException(nameof(categoryForCreateDto));
        }

        var category = await MapToCategoryAsync(categoryForCreateDto);

        await _categoryRepository.CreateAsync(category);

        return MapToCategoryDto(category);
    }

    public async Task<CategoryDto> UpdateCategoryAsync(CategoryForUpdateDto categoryForUpdateDto)
    {
        if (categoryForUpdateDto == null)
        {
            throw new ArgumentNullException(nameof(categoryForUpdateDto));
        }

        var category = await _categoryRepository.FindByIdAsync(categoryForUpdateDto.Id);
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with {categoryForUpdateDto.Id} not found");
        }

        category.CategoryName = categoryForUpdateDto.CategoryName;

        var groups = await _groupRepository.FindByGroupIdsAsync(categoryForUpdateDto.GroupIds);
        category.Groups = groups.ToList();

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
        return new Category
        {
            CategoryName = categoryForCreateDto.CategoryName,
            Groups = groups.ToList()
        };
    }

    private CategoryDto MapToCategoryDto(Category category)
    {
        var groupInfos = category.Groups.Select(g => new GroupInfoResponse(
            g.Id,
            g.GroupName
            )).ToList();

        var serviceDtos = category.Services.Select(s => new ServiceDtos(
            s.Id,
            s.Name,
            s.Amount,
            s.CategoryId,
            s.Category.CategoryName
            )).ToList();

        return new CategoryDto(
            category.Id,
            category.CategoryName,
            groupInfos,
            serviceDtos
        );
    }
}
