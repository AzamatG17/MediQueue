using AutoMapper;
using MediQueue.Domain.DTOs.Role;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;

namespace MediQueue.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly MediQueueDbContext _dbContext;

        public RoleService(IRoleRepository roleRepository, IMapper mapper, MediQueueDbContext mediQueueDbContext)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dbContext = mediQueueDbContext ?? throw new ArgumentNullException(nameof(mediQueueDbContext));
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var role = await _dbContext.Roles
                .Include(x => x.RolePermissions)
                .AsNoTracking()
                .ToListAsync(); 

            var roleDtos = role.Select(role => new RoleDto(
                role.Id,
                role.Name,
                role.RolePermissions.Select(MapToRolePermissionDto).ToList() ?? new List<RolePermissionDto>())
            ).ToList();

            return roleDtos;
        }

        public async Task<RoleDto> GetRoleByIdAsync(int id)
        {
            var role = await _roleRepository.FindByIdAsync(id);

            if (role == null)
            {
                throw new KeyNotFoundException($"Role with {id} not found");
            }

            var roleDto = new RoleDto(
                role.Id,
                role.Name,
                role.RolePermissions.Select(MapToRolePermissionDto).ToList());

            return roleDto;
        }

        public async Task<RoleDto> CreateRoleAsync(RoleForCreateDto roleForCreateDto)
        {
            if (roleForCreateDto == null)
            {
                throw new ArgumentNullException(nameof(roleForCreateDto));
            }

            var role = new Role
            {
                Name = roleForCreateDto.Name,
                RolePermissions = new List<RolePermission>()
            };

            foreach (var rolePermission in roleForCreateDto.PermissionId)
            {
                var rolePerm = new RolePermission
                {
                    ControllerId = rolePermission.ControllerId,
                    Permissions = new List<int>()
                };

                foreach (var permissionId in rolePermission.Permissions)
                {
                    var permission = await _dbContext.Permissions.FindAsync(permissionId);
                    if (permission != null)
                    {
                        rolePerm.Permissions.Add(permission.Id);
                    }
                }

                role.RolePermissions.Add(rolePerm);
            }

            _dbContext.Roles.Add(role);
            await _dbContext.SaveChangesAsync();

            try
            {
                var roleDto = new RoleDto(
               role.Id,
               role.Name,
               role.RolePermissions.Select(MapToRolePermissionDto).ToList());

                return roleDto;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<RoleDto> UpdateRoleAsync(RoleForUpdateDto roleForUpdateDto)
        {
            if (roleForUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(roleForUpdateDto));
            }

            var role = _mapper.Map<Role>(roleForUpdateDto);

            await _roleRepository.UpdateAsync(role);

            var roleDto = new RoleDto(
                Id: role.Id,
                Name: role.Name,
                role.RolePermissions.Select(MapToRolePermissionDto).ToList());

            return roleDto;
        }

        public async Task DeleteRoleAsync(int id)
        {
            await _roleRepository.DeleteAsync(id);
        }

        private RolePermissionDto MapToRolePermissionDto(RolePermission role)
        {
            return new RolePermissionDto(
                role?.ControllerId ?? 0,
                role?.Permissions ?? new List<int>()
                );
        }
    }
}
