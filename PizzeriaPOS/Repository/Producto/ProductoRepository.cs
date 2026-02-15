using Microsoft.EntityFrameworkCore;
using PizzeriaPOS.DTOs;
using PizzeriaPOS.Models;

namespace PizzeriaPOS.Repository
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly PizzeriaPosContext _context;

        public ProductoRepository(PizzeriaPosContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductoDTO>> GetAllAsync()
        {
            try
            {

                var productos = from p in _context.Productos
                                  from c in _context.Categoria.Where(c => c.Id == p.CategoriaId).DefaultIfEmpty()
                                  select new ProductoDTO
                                  {
                                      Id = p.Id,
                                      CategoriaId = p.CategoriaId,
                                      Categoria = c.Nombre,
                                      Nombre = p.Nombre,
                                      Descripcion = p.Descripcion,
                                      Precio = p.Precio,
                                      Stock = p.Stock,
                                      Activo = p.Activo,
                                      FechaCreacion = p.FechaCreacion
                                  };


                return await productos.ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProductoDTO>> GetByCategoriaIdAsync(int categoriaId)
        {
            try
            {
                var productos = from p in _context.Productos.Where(p => p.CategoriaId == categoriaId)
                                from c in _context.Categoria.Where(c => c.Id == p.CategoriaId).DefaultIfEmpty()
                                select new ProductoDTO
                                {
                                    Id = p.Id,
                                    CategoriaId = p.CategoriaId,
                                    Categoria = c.Nombre,
                                    Nombre = p.Nombre,
                                    Descripcion = p.Descripcion,
                                    Precio = p.Precio,
                                    Stock = p.Stock,
                                    Activo = p.Activo,
                                    FechaCreacion = p.FechaCreacion
                                };


                return await productos.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductoDTO?> GetByIdAsync(int Id)
        {
            try
            {
                var producto = await _context.Productos.Where(p => p.Id == Id).FirstOrDefaultAsync();

                if (producto == null)
                    return null;

                var response = new ProductoDTO
                {
                    Id = producto.Id,
                    CategoriaId = producto.CategoriaId,
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio,
                    Stock = producto.Stock,
                    Activo = producto.Activo,
                    FechaCreacion = producto.FechaCreacion
                };

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductoDTO?> CreateAsync(ProductoCreateUpdateDTO request)
        {
            try
            {
                var categoriaExists = await _context.Categoria.AnyAsync(c => c.Id == request.CategoriaId);
                if (!categoriaExists)
                {
                    throw new Exception("La categoría especificada no existe");
                }

                if (await NombreExist(request.Nombre))
                {
                    throw new Exception("Este nombre de producto ya existe");
                }

                var producto = new Producto
                {
                    CategoriaId = request.CategoriaId,
                    Nombre = request.Nombre,
                    Descripcion = request.Descripcion,
                    Precio = request.Precio,
                    Stock = request.Stock,
                    Activo = request.Activo ?? true
                };

                await _context.Productos.AddAsync(producto);
                await _context.SaveChangesAsync();

                return await GetByIdAsync(producto.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductoDTO?> UpdateAsync(ProductoCreateUpdateDTO request, int Id)
        {
            try
            {
                var producto = await _context.Productos.FindAsync(Id);
                if (producto == null)
                    return null;

                var categoriaExists = await _context.Categoria.AnyAsync(c => c.Id == request.CategoriaId);
                if (!categoriaExists)
                {
                    throw new Exception("La categoría especificada no existe");
                }

                if (await NombreExist(request.Nombre, Id))
                {
                    throw new Exception("Este nombre de producto ya existe");
                }

                producto.CategoriaId = request.CategoriaId;
                producto.Nombre = request.Nombre;
                producto.Descripcion = request.Descripcion;
                producto.Precio = request.Precio;
                producto.Stock = request.Stock;
                producto.Activo = request.Activo;

                await _context.SaveChangesAsync();

                return await GetByIdAsync(producto.Id);
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
                var producto = await _context.Productos.FindAsync(Id);
                if (producto == null)
                    return false;

                var hasPedidoDetalles = await _context.PedidoDetalles.AnyAsync(pd => pd.ProductoId == Id);
                if (hasPedidoDetalles)
                {
                    throw new Exception("No se puede eliminar el producto porque tiene pedidos asociados");
                }

                _context.Remove(producto);
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
                var query = _context.Productos.Where(p => p.Nombre == nombre);

                if (id.HasValue)
                    query = query.Where(p => p.Id != id.Value);

                return await query.AnyAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}