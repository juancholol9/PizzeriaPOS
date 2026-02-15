using Microsoft.AspNetCore.Mvc;
using PizzeriaPOS.DTOs;
using PizzeriaPOS.Repository;

namespace PizzeriaPOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteRepository _repository;

        public ClienteController(IClienteRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var clientes = await _repository.GetAllAsync();
            return Ok(new { mensaje = "ok", response = clientes });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var cliente = await _repository.GetByIdAsync(id);

            if (cliente == null)
                return NotFound(new { mensaje = "Cliente no encontrado" });

            return Ok(new { mensaje = "ok", response = cliente });
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ClienteCreateUpdateDTO request)
        {
            try
            {
                var cliente = await _repository.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetByIdAsync),
                    new { id = cliente!.Id },
                    new { mensaje = "Cliente creado correctamente", response = cliente }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensaje = "error",
                    error = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] ClienteCreateUpdateDTO request)
        {
            try
            {
                var cliente = await _repository.UpdateAsync(request, id);

                if (cliente == null)
                    return NotFound(new { mensaje = "Cliente no encontrado" });

                return Ok(new { mensaje = "Cliente actualizado correctamente", response = cliente });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    mensaje = "error",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var deleted = await _repository.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { mensaje = "Cliente no encontrado" });

            return Ok(new { mensaje = "Cliente eliminado correctamente" });
        }
    }
}
