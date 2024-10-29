using KERP.Core.DTOs;
using KERP.Core.Entities;

namespace KERP.Core.Interfaces.Services
{
    public interface IMassUpdatePurchaseService
    {
        Task<MassUpdatePurchase> CreatePurchaseAsync(MassUpdatePurchaseDto dto);
    }
}
