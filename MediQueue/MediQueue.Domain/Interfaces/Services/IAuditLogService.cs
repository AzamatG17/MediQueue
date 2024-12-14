using MediQueue.Domain.Entities;
using MediQueue.Domain.ResourceParameters;

namespace MediQueue.Domain.Interfaces.Services;

public interface IAuditLogService
{
    Task<IEnumerable<AuditLog>> GetAllAuditLogAsync(AuditLogResourceParameters auditLogResourceParameters);
}
