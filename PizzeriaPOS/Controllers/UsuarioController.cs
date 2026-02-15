using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PizzeriaPOS.DTOs;
using PizzeriaPOS.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PizzeriaPOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioController(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var usuarios = await _repository.GetAllAsync();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = usuarios });
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
                var usuario = await _repository.GetByIdAsync(id);
                if (usuario == null)
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "usuario no existe" });

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = usuario });
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

        [HttpGet("username/{nombreUsuario}")]
        public async Task<IActionResult> GetByNombreUsuarioAsync(string nombreUsuario)
        {
            try
            {
                var usuario = await _repository.GetByNombreUsuarioAsync(nombreUsuario);
                if (usuario == null)
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "usuario no existe" });

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = usuario });
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
        public async Task<IActionResult> CreateAsync([FromBody] UsuarioCreateUpdateDTO request)
        {
            try
            {
                var usuario = await _repository.CreateAsync(request);
                return StatusCode(StatusCodes.Status201Created, new { mensaje = "ok", response = usuario });
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

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UsuarioLoginDTO request)
        {
            try
            {
                var usuario = await _repository.ValidateLoginAsync(request);
                if (usuario == null)
                    return StatusCode(StatusCodes.Status401Unauthorized, new { mensaje = "credenciales inválidas" });

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "login exitoso", response = usuario });
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
        public async Task<IActionResult> UpdateAsync([FromBody] UsuarioCreateUpdateDTO request, int id)
        {
            try
            {
                var usuario = await _repository.UpdateAsync(request, id);
                if (usuario == null)
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "usuario no existe" });

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = usuario });
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
                    return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "usuario no existe" });

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