using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class PaymentServiceRepository : RepositoryBase<PaymentService>, IPaymentServiceRepository
    {
        public PaymentServiceRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<PaymentService>> GetAllPaymentServicesAsync()
        {
            return await _context.PaymentServices
                .Include(a => a.Account)
                .Include(s => s.Service)
                .Include(q => q.QuestionnaireHistory)
                .Where(x => x.IsActive)
                .AsNoTracking()
                .ToListAsync(); 
        }

        public async Task<PaymentService> GetByIdPaymentServiceAsync(int id)
        {
            return await _context.PaymentServices
                .Include(a => a.Account)
                .Include(s => s.Service)
                .Include(q => q.QuestionnaireHistory)
                .Where(x => x.Id == id && x.IsActive)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<PaymentService> GetPaymentServiceByIdAsync(int id)
        {
            return await _context.PaymentServices
                .Include(a => a.Account)
                .Include(s => s.Service)
                .Include(q => q.QuestionnaireHistory)
                .Where(x => x.Id == id && x.IsActive)
                .FirstOrDefaultAsync();
        }
    }
}
