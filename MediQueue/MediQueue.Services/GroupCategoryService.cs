using AutoMapper;
using MediQueue.Domain.DTOs.GroupCategory;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services
{
    public class GroupCategoryService : IGroupCategoryService
    {
        private readonly IGroupCategoryRepository _repository;
        private readonly IMapper _mapper;

        public GroupCategoryService(IGroupCategoryRepository groupCategoryRepository, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = groupCategoryRepository ?? throw new ArgumentNullException(nameof(groupCategoryRepository));
        }

        public async Task<IEnumerable<GroupCategoryDto>> GetAllGroupCategoriesAsync()
        {
            var groupCategory = await _repository.FindAllAsync();

             return _mapper.Map<IEnumerable<GroupCategoryDto>>(groupCategory);
        }

        public async Task<GroupCategoryDto> GetGroupCategoryByIdAsync(int id)
        {
            var groupCategory = await _repository.FindByIdAsync(id);
            if (groupCategory == null)
            {
                throw new KeyNotFoundException($"Group Category with {id} not found");
            }

            return _mapper.Map<GroupCategoryDto>(groupCategory);
        }

        public async Task<GroupCategoryDto> CreateGroupCategoryAsync(GroupCategoryForCreate groupCategoryForCreate)
        {
            if (groupCategoryForCreate == null)
            {
                throw new ArgumentNullException(nameof(groupCategoryForCreate));
            }

            var group = _mapper.Map<GroupCategory>(groupCategoryForCreate);

            await _repository.CreateAsync(group);

            return _mapper.Map<GroupCategoryDto>(group);
        }
        
        public async Task<GroupCategoryDto> UpdateGroupCategoryAsync(GroupCategoryForUpdate groupCategoryForUpdate)
        {
            if (groupCategoryForUpdate == null)
            {
                throw new ArgumentNullException(nameof(groupCategoryForUpdate));
            }

            var group = _mapper.Map<GroupCategory>(groupCategoryForUpdate);

            await _repository.UpdateAsync(group);

            return _mapper.Map<GroupCategoryDto>(group);
        }

        public async Task DeleteGroupCategoryAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
