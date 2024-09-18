using AutoMapper;
using MediQueue.Domain.DTOs.Permission;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services
{
    public class PermissonService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;

        public PermissonService(IPermissionRepository permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public Task<PermissionDto> CreatePermissionAsync(PermissionForCreateDto permissionForCreateDto)
        {
            throw new NotImplementedException();
        }

        public Task DeletePermissionAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync()
        {
            var permission = await _permissionRepository.FindAllAsync();

            return _mapper.Map<IEnumerable<PermissionDto>>(permission);
        }

        public async Task<PermissionDto> GetPermissionByIdAsync(int id)
        {
            var permission = await _permissionRepository.FindByIdAsync(id);
            if (permission == null)
            {
                throw new KeyNotFoundException($"Permission with {id} not found");
            }

            return _mapper.Map<PermissionDto>(permission);
        }

        public Task<PermissionDto> UpdatePermissionAsync(PermissionForUpdateDto permissionForUpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}
