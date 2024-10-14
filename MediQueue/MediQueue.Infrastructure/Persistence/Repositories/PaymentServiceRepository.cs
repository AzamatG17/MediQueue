using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.ResourceParameters;
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
                .AsNoTracking()
                .ToListAsync(); 
        }

        public async Task<PaymentService> GetByIdPaymentServiceAsync(int id)
        {
            return await _context.PaymentServices
                .Include(a => a.Account)
                .Include(s => s.Service)
                .Include(q => q.QuestionnaireHistory)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
