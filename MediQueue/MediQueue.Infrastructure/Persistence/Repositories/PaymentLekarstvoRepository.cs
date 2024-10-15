using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediQueue.Infrastructure.Persistence.Repositories
{
    public class PaymentLekarstvoRepository : RepositoryBase<PaymentLekarstvo>, IPaymentLekarstvoRepository
    {
        public PaymentLekarstvoRepository(MediQueueDbContext mediQueueDbContext)
            : base(mediQueueDbContext)
        {
        }

        public async Task<IEnumerable<PaymentLekarstvo>> GetAllPaymentLekarstvosAsync()
        {
            return await _context.PaymentLekarstvos
                .Include(a => a.Account)
                .Include(s => s.Lekarstvo)
                .Include(q => q.QuestionnaireHistory)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PaymentLekarstvo> GetByIdPaymentLekarstvoAsync(int id)
        {
            return await _context.PaymentLekarstvos
                .Include(a => a.Account)
                .Include(s => s.Lekarstvo)
                .Include(q => q.QuestionnaireHistory)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
