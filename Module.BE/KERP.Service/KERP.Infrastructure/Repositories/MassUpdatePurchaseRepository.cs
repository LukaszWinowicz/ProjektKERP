using KERP.Core.Entities;
using KERP.Core.Interfaces.Repositories;
using KERP.Infrastructure.Database;

namespace KERP.Infrastructure.Repositories
{
    public class MassUpdatePurchaseRepository : IMassUpdatePurchaseRepository
    {
        private readonly ServiceDbContext _context;

        public MassUpdatePurchaseRepository(ServiceDbContext context)
        {
            _context = context;
        }

        public async Task AddPurchaseAsync(MassUpdatePurchase newPurchase)
        {
            await _context.MassUpdatePurchase.AddAsync(newPurchase);
            await _context.SaveChangesAsync();
        }
    }
}
