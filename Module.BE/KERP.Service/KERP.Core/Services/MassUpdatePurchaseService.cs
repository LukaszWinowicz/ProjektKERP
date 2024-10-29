using KERP.Core.DTOs;
using KERP.Core.Entities;
using KERP.Core.Interfaces.Repositories;
using KERP.Core.Interfaces.Services;

namespace KERP.Core.Services
{
    public class MassUpdatePurchaseService : IMassUpdatePurchaseService
    {
        private readonly IMassUpdatePurchaseRepository _repository;

        // Tymczasowa lista istniejących obiektów, domyslnie będzie połączenie z tablicą w bazie danych a metody CheckThatPurchaseOrderExist
        // oraz CheckThatConcatenationExist będą przeniesione do repo
        private List<MassUpdatePurchaseDto> existingObjects = new List<MassUpdatePurchaseDto>
        {
            new MassUpdatePurchaseDto {PurchaseOrderNumber = "123456789", LineNumber = 10, Sequence = 1 },
            new MassUpdatePurchaseDto {PurchaseOrderNumber = "123456789", LineNumber = 20, Sequence = 2 },
            new MassUpdatePurchaseDto {PurchaseOrderNumber = "123ab6789", LineNumber = 10, Sequence = 1 },
        };

        public MassUpdatePurchaseService(IMassUpdatePurchaseRepository repository)
        {
            _repository = repository;
        }

        public async Task<MassUpdatePurchase> CreatePurchaseAsync(MassUpdatePurchaseDto dto)
        {
            // Sprawdzenie czy PurchaseOrder istnieje
            if (!CheckThatPurchaseOrderExist(dto))
            {
                throw new ArgumentException("Podany Purchase Order nie istnieje.");
            }

            // Sprawdzenie czy kombinacja PurchaseOrder, LineNumber, Sequence istnieje
            if (!CheckThatConcatenationExist(dto))
            {
                throw new ArgumentException("Brak lini z podanymi parametrmia: Purchase Order / Line Number / Sequence");
            }

            // Mapowanie DTO na encję - później przenieść to do AutoMappera
            var newPurchase = new MassUpdatePurchase
            {
                PurchaseOrderNumber = dto.PurchaseOrderNumber,
                LineNumber = dto.LineNumber,
                Sequence = dto.Sequence,
                ConfirmedReceiptDate = dto.ConfirmedReceiptDate,
                ChangedReceiptDate = dto.ChangedReceiptDate,
                AddedByUserId = 1,    // Wartość ustawiona tymczasowo, domyślnie powinno pobierać UserId
                IsGenerated = false,  // Domyślna wartość
                GeneratedDate = null  // Domyślnie null
            };

            await _repository.AddPurchaseAsync(newPurchase);
            return newPurchase;

        }

        // Sprawdzenie czy PurchaseOrder istnieje
        public bool CheckThatPurchaseOrderExist(MassUpdatePurchaseDto dto)
        {
            return existingObjects.Any(x => x.PurchaseOrderNumber == dto.PurchaseOrderNumber);
        }

        // Sprawdzenie czy kombinacja PurchaseOrder, LineNumber i Sequence istnieje
        public bool CheckThatConcatenationExist(MassUpdatePurchaseDto dto)
        {
            return existingObjects.Any(x => x.PurchaseOrderNumber == dto.PurchaseOrderNumber && x.LineNumber == dto.LineNumber && x.Sequence == dto.Sequence);
        }
    }
}
