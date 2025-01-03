using AutoMapper;
using MediQueue.Domain.DTOs.Permission;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Exceptions;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Services;

public class PermissonService : IPermissionService
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IMapper _mapper;
    private readonly MediQueueDbContext _dbContext;

    public PermissonService(IPermissionRepository permissionRepository, IMapper mapper, MediQueueDbContext mediQueueDbContext)
    {
        _permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _dbContext = mediQueueDbContext ?? throw new ArgumentNullException(nameof(mediQueueDbContext));
    }

    public async Task<(IEnumerable<Controller>, IEnumerable<PermissionDto>)> GetAllPermissionsAsync()
    {
        var permission = await _permissionRepository.FindAllAsync();

        var controllers = await GetAllControllers();

        var permissionDtos = _mapper.Map<IEnumerable<PermissionDto>>(permission);

        return (controllers, permissionDtos);
    }

    public async Task<PermissionDto> GetPermissionByIdAsync(int id)
    {
        var permission = await _permissionRepository.FindByIdAsync(id)
            ?? throw new EntityNotFoundException($"Permission with {id} not found");

        return _mapper.Map<PermissionDto>(permission);
    }

    public Task<PermissionDto> CreatePermissionAsync(PermissionForCreateDto permissionForCreateDto)
    {
        throw new NotImplementedException();
    }

    public Task<PermissionDto> UpdatePermissionAsync(PermissionForUpdateDto permissionForUpdateDto)
    {
        throw new NotImplementedException();
    }

    public Task DeletePermissionAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Controller>> GetAllControllers()
    {
        return await _dbContext.Controllers
            .AsNoTracking()
            .Where(x => x.IsActive)
            .ToListAsync();
    }
}
