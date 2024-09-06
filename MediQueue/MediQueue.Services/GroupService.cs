using AutoMapper;
using MediQueue.Domain.DTOs.Group;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GroupService(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<GroupDto>> GetAllGroupsAsync()
        {
            var group = _groupRepository.FindAllAsync();

            return _mapper.Map<IEnumerable<GroupDto>>(group);
        }

        public async Task<GroupDto> GetGroupByIdAsync(int id)
        {
            var group = _groupRepository.FindByIdAsync(id);
            if (group == null)
            {
                throw new KeyNotFoundException($"Group with {id} not found");
            }

            return _mapper.Map<GroupDto>(group);
        }

        public async Task<GroupDto> CreateGroupAsync(GroupForCreateDto groupForCreateDto)
        {
            if (groupForCreateDto == null)
            {
                throw new ArgumentNullException(nameof(groupForCreateDto));
            }

            var group = _mapper.Map<Group>(groupForCreateDto);

            await _groupRepository.CreateAsync(group);

            return _mapper.Map<GroupDto>(group);
        }

        public async Task<GroupDto> UpdateGroupAsync(GroupForUpdateDto groupForUpdateDto)
        {
            if (groupForUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(groupForUpdateDto));
            }

            var group = _mapper.Map<Group>(groupForUpdateDto);

            await _groupRepository.UpdateAsync(group);

            return _mapper.Map<GroupDto>(group);
        }

        public async Task DeleteGroupAsync(int id)
        {
            await _groupRepository.DeleteAsync(id); 
        }
    }
}
