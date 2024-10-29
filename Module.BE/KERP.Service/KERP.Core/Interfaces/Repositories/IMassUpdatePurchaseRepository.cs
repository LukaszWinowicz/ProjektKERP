using KERP.Core.Entities;

namespace KERP.Core.Interfaces.Repositories
{
    public interface IMassUpdatePurchaseRepository
    {
        Task AddPurchaseAsync(MassUpdatePurchase newPurchase);
    }
}
