using KERP.Core.DTOs;
using KERP.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace KERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MassUpdatePurchaseController : ControllerBase
    {
        private readonly IMassUpdatePurchaseService _service;

        public MassUpdatePurchaseController(IMassUpdatePurchaseService service) 
        { 
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> CreatePurchase([FromBody] MassUpdatePurchaseDto dto)
        {
            // Sprawdzenie, czy dane wejściowe są poprawne
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Przekazanie danych do serwisu w celu weryfikacji i utworzenia rekordu
                var newPurchase = await _service.CreatePurchaseAsync(dto);
                return CreatedAtAction(nameof(CreatePurchase), new { id = newPurchase.Id }, newPurchase);

            }
            catch (ArgumentException ex)
            {
                // Obsługa błędów, np. nieistniejący PurchaseOrder lub nieistniejąca kombinacja
                return BadRequest(ex.Message);

            }
        }
    }
}
