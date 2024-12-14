using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Domain.ResourceParameters;
using MediQueue.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Services;

public class AuditLogService : IAuditLogService
{
    private readonly MediQueueDbContext _context;

    public AuditLogService(MediQueueDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<AuditLog>> GetAllAuditLogAsync(AuditLogResourceParameters auditLogResourceParameters)
    {
        var query = _context.AuditLogs
            .AsNoTracking()
            .AsQueryable();

        if (auditLogResourceParameters.Id.HasValue)
        {
            query = query.Where(q => q.Id == auditLogResourceParameters.Id.Value);
        }
        if (auditLogResourceParameters.AccountId.HasValue)
        {
            query = query.Where(q => q.AccountId == auditLogResourceParameters.AccountId.Value);
        }
        if (auditLogResourceParameters.StartDateTime.HasValue)
        {
            query = query.Where(q => q.Timestamp >= auditLogResourceParameters.StartDateTime.Value);
        }
        if (auditLogResourceParameters.EndDateTime.HasValue)
        {
            query = query.Where(q => q.Timestamp >= auditLogResourceParameters.EndDateTime.Value);
        }
        if (!string.IsNullOrEmpty(auditLogResourceParameters.TableName))
        {
            query = query.Where(q => q.TableName.Contains(auditLogResourceParameters.TableName));
        }

        if (auditLogResourceParameters.Action.HasValue)
        {
            query = query.Where(q => q.Action == auditLogResourceParameters.Action.ToString());
        }

        query = auditLogResourceParameters.OrderBy switch
        {
            "idDesc" => query.OrderByDescending(q => q.Id),
            "idAsc" => query.OrderBy(q => q.Id),
            _ => query
        };

        return await query.ToListAsync();
    }
}
