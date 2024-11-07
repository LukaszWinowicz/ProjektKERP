using KERP.Client.DTOs.MassUpdate;

namespace KERP.Client.Services
{
    public class MassUpdatePurchaseApi
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:5000/api";

        // Dane tymczasowe, następnym krokiem, będzie porównywanie ich 
        private List<MassUpdatePurchaseDto> existingObjects = new List<MassUpdatePurchaseDto>
        {
            new MassUpdatePurchaseDto {PurchaseOrder = "123456789", LineNumber = 10, Sequence = 1 },
            new MassUpdatePurchaseDto {PurchaseOrder = "123456789", LineNumber = 20, Sequence = 2 },
            new MassUpdatePurchaseDto {PurchaseOrder = "123ab6789", LineNumber = 10, Sequence = 1 },
         };

        public MassUpdatePurchaseApi(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        /*
         * Metoda do sprawdzania czy dane PO lub kombinacja PO-line-seq istnieją
         * Jeśli oba warunki zostaną spełnione to zwracane jest true, jeśli przynajmniej 
         * jeden nie jest to zwracane jest false
         */
        public bool CheckThatPurchaseOrderExist(MassUpdatePurchaseDto model) 
        {
            bool purchaseOrderExists = existingObjects.Any(x => x.PurchaseOrder == model.PurchaseOrder);
            if (purchaseOrderExists)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        public bool CheckThatConcatenationExist(MassUpdatePurchaseDto model)
        {
            bool concatenationExists = existingObjects.Any(x => x.PurchaseOrder == model.PurchaseOrder && x.LineNumber == model.LineNumber && x.Sequence == model.Sequence);
            if (concatenationExists)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
    }
}
