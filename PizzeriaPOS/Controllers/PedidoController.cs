using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzeriaPOS.DTOs;
using PizzeriaPOS.Repository;

namespace PizzeriaPOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoRepository _repository;

        public PedidoController(IPedidoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var pedidos = await _repository.GetAllAsync();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = pedidos });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    mensaje = "error",
                    error = ex.Message,
                    detalles = ex.ToString()
                });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var pedido = await _repository.GetByIdAsync(id);
                if (pedido == null)
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "pedido no existe" });

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = pedido });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    mensaje = "error",
                    error = ex.Message,
                    detalles = ex.ToString()
                });
            }
        }

        [HttpGet("cliente/{clienteId:int}")]
        public async Task<IActionResult> GetByClienteIdAsync(int clienteId)
        {
            try
            {
                var pedidos = await _repository.GetByClienteIdAsync(clienteId);
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = pedidos });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    mensaje = "error",
                    error = ex.Message,
                    detalles = ex.ToString()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] PedidoCreateUpdateDTO request)
        {
            try
            {
                var pedido = await _repository.CreateAsync(request);
                return StatusCode(StatusCodes.Status201Created, new { mensaje = "ok", response = pedido });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    mensaje = "error",
                    error = ex.Message,
                    detalles = ex.ToString()
                });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync([FromBody] PedidoCreateUpdateDTO request, int id)
        {
            try
            {
                var pedido = await _repository.UpdateAsync(request, id);
                if (pedido == null)
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "pedido no existe" });

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = pedido });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    mensaje = "error",
                    error = ex.Message,
                    detalles = ex.ToString()
                });
            }
        }

        [HttpPatch("{id:int}/estado")]
        public async Task<IActionResult> UpdateEstadoAsync(int id, [FromBody] UpdateEstadoRequest request)
        {
            try
            {
                var result = await _repository.UpdateEstadoAsync(id, request.Estado);
                if (result == false)
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "pedido no existe" });

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "estado actualizado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    mensaje = "error",
                    error = ex.Message,
                    detalles = ex.ToString()
                });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _repository.DeleteAsync(id);
                if (result == false)
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "pedido no existe" });

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    mensaje = "error",
                    error = ex.Message,
                    detalles = ex.ToString()
                });
            }
        }
    }

    public class UpdateEstadoRequest
    {
        public string Estado { get; set; } = string.Empty;
    }
}