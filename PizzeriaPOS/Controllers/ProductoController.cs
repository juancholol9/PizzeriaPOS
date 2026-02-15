using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzeriaPOS.DTOs;
using PizzeriaPOS.Repository;

namespace PizzeriaPOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoRepository _repository;

        public ProductoController(IProductoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var productos = await _repository.GetAllAsync();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = productos });
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
                var producto = await _repository.GetByIdAsync(id);
                if (producto == null)
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "producto no existe" });

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = producto });
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

        [HttpGet("categoria/{categoriaId:int}")]
        public async Task<IActionResult> GetByCategoriaIdAsync(int categoriaId)
        {
            try
            {
                var productos = await _repository.GetByCategoriaIdAsync(categoriaId);
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = productos });
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
        public async Task<IActionResult> CreateAsync([FromBody] ProductoCreateUpdateDTO request)
        {
            try
            {
                var producto = await _repository.CreateAsync(request);
                return StatusCode(StatusCodes.Status201Created, new { mensaje = "ok", response = producto });
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
        public async Task<IActionResult> UpdateAsync([FromBody] ProductoCreateUpdateDTO request, int id)
        {
            try
            {
                var producto = await _repository.UpdateAsync(request, id);
                if (producto == null)
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "producto no existe" });

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = producto });
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
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "producto no existe" });

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