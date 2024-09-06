using AutoMapper;
using MediQueue.Domain.DTOs.Role;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;
using System;

namespace MediQueue.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var role = await _roleRepository.FindAllAsync();

            return _mapper.Map<IEnumerable<RoleDto>>(role);
        }

        public async Task<RoleDto> GetRoleByIdAsync(int id)
        {
            var role = await _roleRepository.FindByIdAsync(id);

            if (role == null)
            {
                throw new KeyNotFoundException($"Role with {id} not found");
            }

            return _mapper.Map<RoleDto>(role);
        }

        public async Task<RoleDto> CreateRoleAsync(RoleForCreateDto roleForCreateDto)
        {
            if (roleForCreateDto == null)
            {
                throw new ArgumentNullException(nameof(roleForCreateDto));
            }

            var role = _mapper.Map<Role>(roleForCreateDto);

            await _roleRepository.CreateAsync(role);

            return _mapper.Map<RoleDto>(role);
        }

        public async Task<RoleDto> UpdateRoleAsync(RoleForUpdateDto roleForUpdateDto)
        {
            if (roleForUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(roleForUpdateDto));
            }

            var role = _mapper.Map<Role>(roleForUpdateDto);

            await _roleRepository.UpdateAsync(role);

            return _mapper.Map<RoleDto>(role);
        }

        public async Task DeleteRoleAsync(int id)
        {
            await _roleRepository.DeleteAsync(id);
        }
    }
}
