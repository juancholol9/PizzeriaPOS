using Microsoft.EntityFrameworkCore;
using PizzeriaPOS.DTOs;
using PizzeriaPOS.Models;

namespace PizzeriaPOS.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly PizzeriaPosContext _context;

        public CategoriaRepository(PizzeriaPosContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoriaDTO>> GetAllAsync()
        {
            try
            {
                var categorias = await _context.Categoria.ToListAsync();

                var response = categorias.Select(c => new CategoriaDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    Activa = c.Activa
                });

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CategoriaDTO?> GetByIdAsync(int Id)
        {
            try
            {
                var categoria = await _context.Categoria.Where(c => c.Id == Id).FirstOrDefaultAsync();

                if (categoria == null)
                    return null;

                var response = new CategoriaDTO
                {
                    Id = categoria.Id,
                    Nombre = categoria.Nombre,
                    Descripcion = categoria.Descripcion,
                    Activa = categoria.Activa
                };

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CategoriaDTO?> CreateAsync(CategoriaCreateUpdateDTO request)
        {
            try
            {
                if (await NombreExist(request.Nombre))
                {
                    throw new Exception("Este nombre de categoría ya existe");
                }

                var categoria = new Categoria
                {
                    Nombre = request.Nombre,
                    Descripcion = request.Descripcion,
                    Activa = request.Activa ?? true
                };

                await _context.Categoria.AddAsync(categoria);
                await _context.SaveChangesAsync();

                return await GetByIdAsync(categoria.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CategoriaDTO?> UpdateAsync(CategoriaCreateUpdateDTO request, int Id)
        {
            try
            {
                var categoria = await _context.Categoria.FindAsync(Id);
                if (categoria == null)
                    return null;

                if (await NombreExist(request.Nombre, Id))
                {
                    throw new Exception("Este nombre de categoría ya existe");
                }

                categoria.Nombre = request.Nombre;
                categoria.Descripcion = request.Descripcion;
                categoria.Activa = request.Activa;

                await _context.SaveChangesAsync();

                return await GetByIdAsync(categoria.Id);
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
                var categoria = await _context.Categoria.FindAsync(Id);
                if (categoria == null)
                    return false;

                // Check if category has products
                var hasProducts = await _context.Productos.AnyAsync(p => p.CategoriaId == Id);
                if (hasProducts)
                {
                    throw new Exception("No se puede eliminar la categoría porque tiene productos asociados");
                }

                _context.Remove(categoria);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<bool> NombreExist(string nombre, int? id = null)
        {
            try
            {
                var query = _context.Categoria.Where(c => c.Nombre == nombre);

                if (id.HasValue)
                    query = query.Where(c => c.Id != id.Value);

                return await query.AnyAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}