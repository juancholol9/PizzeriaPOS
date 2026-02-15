using Microsoft.EntityFrameworkCore;
using PizzeriaPOS.DTOs;
using PizzeriaPOS.Models;
using BCrypt.Net;

namespace PizzeriaPOS.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly PizzeriaPosContext _context;

        public UsuarioRepository(PizzeriaPosContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UsuarioDTO>> GetAllAsync()
        {
            try
            {
                var usuarios = await _context.Usuarios.ToListAsync();

                var response = usuarios.Select(u => new UsuarioDTO
                {
                    Id = u.Id,
                    NombreUsuario = u.NombreUsuario,
                    Email = u.Email,
                    Rol = u.Rol,
                    Activo = u.Activo,
                    FechaCreacion = u.FechaCreacion
                });

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UsuarioDTO?> GetByIdAsync(int Id)
        {
            try
            {
                var usuario = await _context.Usuarios.Where(u => u.Id == Id).FirstOrDefaultAsync();

                if (usuario == null)
                    return null;

                var response = new UsuarioDTO
                {
                    Id = usuario.Id,
                    NombreUsuario = usuario.NombreUsuario,
                    Email = usuario.Email,
                    Rol = usuario.Rol,
                    Activo = usuario.Activo,
                    FechaCreacion = usuario.FechaCreacion
                };

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UsuarioDTO?> GetByNombreUsuarioAsync(string nombreUsuario)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .Where(u => u.NombreUsuario == nombreUsuario)
                    .FirstOrDefaultAsync();

                if (usuario == null)
                    return null;

                var response = new UsuarioDTO
                {
                    Id = usuario.Id,
                    NombreUsuario = usuario.NombreUsuario,
                    Email = usuario.Email,
                    Rol = usuario.Rol,
                    Activo = usuario.Activo,
                    FechaCreacion = usuario.FechaCreacion
                };

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UsuarioDTO?> CreateAsync(UsuarioCreateUpdateDTO request)
        {
            try
            {
                if (await NombreUsuarioExist(request.NombreUsuario))
                {
                    throw new Exception("Este nombre de usuario ya existe");
                }

                if (await EmailExist(request.Email))
                {
                    throw new Exception("Este email ya existe");
                }

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

                var usuario = new Usuario
                {
                    NombreUsuario = request.NombreUsuario,
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    Rol = request.Rol,
                    Activo = request.Activo ?? true
                };

                await _context.Usuarios.AddAsync(usuario);
                await _context.SaveChangesAsync();

                return await GetByIdAsync(usuario.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UsuarioDTO?> UpdateAsync(UsuarioCreateUpdateDTO request, int Id)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(Id);
                if (usuario == null)
                    return null;

                if (await NombreUsuarioExist(request.NombreUsuario, Id))
                {
                    throw new Exception("Este nombre de usuario ya existe");
                }

                if (await EmailExist(request.Email, Id))
                {
                    throw new Exception("Este email ya existe");
                }

                usuario.NombreUsuario = request.NombreUsuario;
                usuario.Email = request.Email;
                usuario.Rol = request.Rol;
                usuario.Activo = request.Activo;

                if (!string.IsNullOrEmpty(request.Password))
                {
                     usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

                }

                await _context.SaveChangesAsync();

                return await GetByIdAsync(usuario.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int Id)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(Id);
                if (usuario == null)
                    return false;

                var hasPedidos = await _context.Pedidos.AnyAsync(p => p.UsuarioId == Id);
                if (hasPedidos)
                {
                    throw new Exception("No se puede eliminar el usuario porque tiene pedidos asociados");
                }

                _context.Remove(usuario);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UsuarioDTO?> ValidateLoginAsync(UsuarioLoginDTO request)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .Where(u => u.NombreUsuario == request.NombreUsuario && u.Activo == true)
                    .FirstOrDefaultAsync();

                if (usuario == null)
                    return null;

                 bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash);

                if (!isValidPassword)
                    return null;

                var response = new UsuarioDTO
                {
                    Id = usuario.Id,
                    NombreUsuario = usuario.NombreUsuario,
                    Email = usuario.Email,
                    Rol = usuario.Rol,
                    Activo = usuario.Activo,
                    FechaCreacion = usuario.FechaCreacion
                };

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<bool> NombreUsuarioExist(string nombreUsuario, int? id = null)
        {
            try
            {
                var query = _context.Usuarios.Where(u => u.NombreUsuario == nombreUsuario);

                if (id.HasValue)
                    query = query.Where(u => u.Id != id.Value);

                return await query.AnyAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<bool> EmailExist(string email, int? id = null)
        {
            try
            {
                var query = _context.Usuarios.Where(u => u.Email == email);

                if (id.HasValue)
                    query = query.Where(u => u.Id != id.Value);

                return await query.AnyAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}