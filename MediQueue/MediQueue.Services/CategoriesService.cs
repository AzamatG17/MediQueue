using AutoMapper;
using MediQueue.Domain.DTOs.Category;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services
{
    public class CategoriesService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.FindAllAsync();

            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.FindByIdAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with {id} not found");
            }

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryForCreateDto categoryForCreateDto)
        {
            if (categoryForCreateDto == null)
            {
                throw new ArgumentNullException(nameof(categoryForCreateDto));
            }

            var category = _mapper.Map<Category>(categoryForCreateDto);

            await _categoryRepository.CreateAsync(category);

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> UpdateCategoryAsync(CategoryForUpdateDto categoryForUpdateDto)
        {
            if (categoryForUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(categoryForUpdateDto));
            }

            var category = _mapper.Map<Category>(categoryForUpdateDto);

            await _categoryRepository.UpdateAsync(category);

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _categoryRepository.DeleteAsync(id);
        }
    }
}
