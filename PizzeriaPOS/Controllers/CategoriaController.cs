using Microsoft.AspNetCore.Mvc;
using PizzeriaPOS.DTOs;
using PizzeriaPOS.Repository;

namespace PizzeriaPOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaRepository _repository;

        public CategoriaController(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
                var categorias = await _repository.GetAllAsync();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = categorias });
            //try
            //{
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, new
            //    {
            //        mensaje = "error",
            //        error = ex.Message,
            //        detalles = ex.ToString()
            //    });
            //}
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var categoria = await _repository.GetByIdAsync(id);
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = categoria });
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
        public async Task<IActionResult> CreateAsync([FromBody] CategoriaCreateUpdateDTO request)
        {
            try
            {
                var categoria = await _repository.CreateAsync(request);
                return StatusCode(StatusCodes.Status201Created, new { mensaje = "ok", response = categoria });
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

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateAsync([FromBody] CategoriaCreateUpdateDTO request, int Id)
        {
            try
            {
                var categoria = await _repository.UpdateAsync(request, Id);
                return StatusCode(StatusCodes.Status201Created, new { mensaje = "ok", response = categoria });
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

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAsync(int Id)
        {
            try
            {
               
                var categoria = await _repository.DeleteAsync(Id);
                if (categoria == false)
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "categoria no existe" });
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok"});
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
