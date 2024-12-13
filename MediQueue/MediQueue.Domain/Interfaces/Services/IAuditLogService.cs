using MediQueue.Domain.Entities;

namespace MediQueue.Domain.Interfaces.Services;

public interface IAuditLogService
{
    Task<IEnumerable<AuditLog>> GetAllLogAsync();
    Task<IEnumerable<AuditLog>> GetByIdLogsAsync(int id);
    Task LogAsync(AuditLog log);
}
