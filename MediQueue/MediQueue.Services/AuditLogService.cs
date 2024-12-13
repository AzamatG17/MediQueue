using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Services;
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

    public async Task<IEnumerable<AuditLog>> GetAllLogAsync()
    {
        return await _context.AuditLogs
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync();
    }

    public Task<IEnumerable<AuditLog>> GetByIdLogsAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task LogAsync(AuditLog log)
    {
        _context.AuditLogs.Add(log);
        await _context.SaveChangesAsync();
    }
}
