using Microsoft.AspNetCore.Http;
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
            try
            {
                var clientes = await _repository.GetAllAsync();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = clientes });
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
                var cliente = await _repository.GetByIdAsync(id);
                if (cliente == null)
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "cliente no existe" });

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = cliente });
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
        public async Task<IActionResult> CreateAsync([FromBody] ClienteCreateUpdateDTO request)
        {
            try
            {
                var cliente = await _repository.CreateAsync(request);
                return StatusCode(StatusCodes.Status201Created, new { mensaje = "ok", response = cliente });
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
        public async Task<IActionResult> UpdateAsync([FromBody] ClienteCreateUpdateDTO request, int id)
        {
            try
            {
                var cliente = await _repository.UpdateAsync(request, id);
                if (cliente == null)
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "cliente no existe" });

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = cliente });
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
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "cliente no existe" });

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
}