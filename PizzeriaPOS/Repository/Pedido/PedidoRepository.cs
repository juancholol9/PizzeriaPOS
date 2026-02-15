using Microsoft.EntityFrameworkCore;
using PizzeriaPOS.DTOs;
using PizzeriaPOS.Models;

namespace PizzeriaPOS.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly PizzeriaPosContext _context;

        public PedidoRepository(PizzeriaPosContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PedidoDTO>> GetAllAsync()
        {
            try
            {

                var pedidios = from p in _context.Pedidos
                               from d in _context.Direccions.Where(d => d.Id == p.DireccionId).DefaultIfEmpty()
                               from c in _context.Clientes.Where(c => c.Id == p.ClienteId).DefaultIfEmpty()
                               from u in _context.Usuarios.Where(u => u.Id == p.UsuarioId).DefaultIfEmpty()
                               select new PedidoDTO
                               {
                                   Id = p.Id,
                                   ClienteId = p.ClienteId,
                                   Cliente = c.Nombre,
                                   DireccionId = p.DireccionId,
                                   Direccion = d.Calle + ", " + d.Referencia + ", " + d.Ciudad,
                                   UsuarioId = p.UsuarioId,
                                   NombreUsuario = u.NombreUsuario,
                                   Fecha = p.Fecha,
                                   Total = p.Total,
                                   Estado = p.Estado
                               };
                               

                return await pedidios.ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<PedidoDTO>> GetByClienteIdAsync(int clienteId)
        {
            try
            {
                var pedidos = await _context.Pedidos
                    .Where(p => p.ClienteId == clienteId)
                    .ToListAsync();

                var response = pedidos.Select(p => new PedidoDTO
                {
                    Id = p.Id,
                    ClienteId = p.ClienteId,
                    DireccionId = p.DireccionId,
                    UsuarioId = p.UsuarioId,
                    Fecha = p.Fecha,
                    Total = p.Total,
                    Estado = p.Estado
                });

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PedidoDetailDTO?> GetByIdAsync(int Id)
        {
            try
            {
                var pedido = await _context.Pedidos
                    .Include(p => p.Cliente)
                    .Include(p => p.Direccion)
                    .Include(p => p.Usuario)
                    .Include(p => p.PedidoDetalles)
                        .ThenInclude(pd => pd.Producto)
                            .ThenInclude(pr => pr.Categoria)
                    .Where(p => p.Id == Id)
                    .FirstOrDefaultAsync();

                if (pedido == null)
                    return null;

                var response = new PedidoDetailDTO
                {
                    Id = pedido.Id,
                    ClienteId = pedido.ClienteId,
                    DireccionId = pedido.DireccionId,
                    UsuarioId = pedido.UsuarioId,
                    Fecha = pedido.Fecha,
                    Total = pedido.Total,
                    Estado = pedido.Estado,
                    Cliente = new ClienteDTO
                    {
                        Id = pedido.Cliente.Id,
                        Nombre = pedido.Cliente.Nombre,
                        Telefono = pedido.Cliente.Telefono,
                        Email = pedido.Cliente.Email
                    },
                    Direccion = new DireccionDTO
                    {
                        Id = pedido.Direccion.Id,
                        ClienteId = pedido.Direccion.ClienteId,
                        Calle = pedido.Direccion.Calle,
                        Ciudad = pedido.Direccion.Ciudad,
                        Referencia = pedido.Direccion.Referencia,
                        Activa = pedido.Direccion.Activa
                    },
                    Usuario = new UsuarioDTO
                    {
                        Id = pedido.Usuario.Id,
                        NombreUsuario = pedido.Usuario.NombreUsuario,
                        Email = pedido.Usuario.Email,
                        Rol = pedido.Usuario.Rol,
                        Activo = pedido.Usuario.Activo,
                        FechaCreacion = pedido.Usuario.FechaCreacion
                    },
                    Detalles = pedido.PedidoDetalles.Select(pd => new PedidoDetalleDetailDTO
                    {
                        Id = pd.Id,
                        PedidoId = pd.PedidoId,
                        ProductoId = pd.ProductoId,
                        Cantidad = pd.Cantidad,
                        PrecioUnitario = pd.PrecioUnitario,
                        SubTotal = pd.SubTotal,
                        Producto = new ProductoDTO
                        {
                            Id = pd.Producto.Id,
                            CategoriaId = pd.Producto.CategoriaId,
                            Nombre = pd.Producto.Nombre,
                            Descripcion = pd.Producto.Descripcion,
                            Precio = pd.Producto.Precio,
                            Stock = pd.Producto.Stock,
                            Activo = pd.Producto.Activo,
                            FechaCreacion = pd.Producto.FechaCreacion
                        }
                    }).ToList()
                };

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PedidoDetailDTO?> CreateAsync(PedidoCreateUpdateDTO request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var clienteExists = await _context.Clientes.AnyAsync(c => c.Id == request.ClienteId);
                if (!clienteExists)
                    throw new Exception("El cliente especificado no existe");

                var direccionExists = await _context.Direccions.AnyAsync(d => d.Id == request.DireccionId);
                if (!direccionExists)
                    throw new Exception("La dirección especificada no existe");

                var usuarioExists = await _context.Usuarios.AnyAsync(u => u.Id == request.UsuarioId);
                if (!usuarioExists)
                    throw new Exception("El usuario especificado no existe");

                foreach (var detalle in request.Detalles)
                {
                    var producto = await _context.Productos.FindAsync(detalle.ProductoId);
                    if (producto == null)
                        throw new Exception($"El producto con ID {detalle.ProductoId} no existe");

                    if (producto.Stock < detalle.Cantidad)
                        throw new Exception($"Stock insuficiente para el producto {producto.Nombre}. Stock disponible: {producto.Stock}");
                }

                decimal total = 0;
                var detalles = new List<PedidoDetalle>();

                foreach (var detalleDTO in request.Detalles)
                {
                    decimal subtotal = detalleDTO.Cantidad * detalleDTO.PrecioUnitario;
                    total += subtotal;

                    var detalle = new PedidoDetalle
                    {
                        ProductoId = detalleDTO.ProductoId,
                        Cantidad = detalleDTO.Cantidad,
                        PrecioUnitario = detalleDTO.PrecioUnitario,
                        SubTotal = subtotal
                    };

                    detalles.Add(detalle);

                    var producto = await _context.Productos.FindAsync(detalleDTO.ProductoId);
                    if (producto != null)
                    {
                        producto.Stock -= detalleDTO.Cantidad;
                    }
                }

                var pedido = new Pedido
                {
                    ClienteId = request.ClienteId,
                    DireccionId = request.DireccionId,
                    UsuarioId = request.UsuarioId,
                    Total = total,
                    Estado = request.Estado ?? "Pendiente",
                    PedidoDetalles = detalles
                };

                await _context.Pedidos.AddAsync(pedido);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return await GetByIdAsync(pedido.Id);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<PedidoDetailDTO?> UpdateAsync(PedidoCreateUpdateDTO request, int Id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var pedido = await _context.Pedidos
                    .Include(p => p.PedidoDetalles)
                    .Where(p => p.Id == Id)
                    .FirstOrDefaultAsync();

                if (pedido == null)
                    return null;

                var clienteExists = await _context.Clientes.AnyAsync(c => c.Id == request.ClienteId);
                if (!clienteExists)
                    throw new Exception("El cliente especificado no existe");

                var direccionExists = await _context.Direccions.AnyAsync(d => d.Id == request.DireccionId);
                if (!direccionExists)
                    throw new Exception("La dirección especificada no existe");

                var usuarioExists = await _context.Usuarios.AnyAsync(u => u.Id == request.UsuarioId);
                if (!usuarioExists)
                    throw new Exception("El usuario especificado no existe");

                foreach (var oldDetalle in pedido.PedidoDetalles)
                {
                    var producto = await _context.Productos.FindAsync(oldDetalle.ProductoId);
                    if (producto != null)
                    {
                        producto.Stock += oldDetalle.Cantidad;
                    }
                }

                _context.PedidoDetalles.RemoveRange(pedido.PedidoDetalles);

                foreach (var detalle in request.Detalles)
                {
                    var producto = await _context.Productos.FindAsync(detalle.ProductoId);
                    if (producto == null)
                        throw new Exception($"El producto con ID {detalle.ProductoId} no existe");

                    if (producto.Stock < detalle.Cantidad)
                        throw new Exception($"Stock insuficiente para el producto {producto.Nombre}. Stock disponible: {producto.Stock}");
                }

                decimal total = 0;
                var newDetalles = new List<PedidoDetalle>();

                foreach (var detalleDTO in request.Detalles)
                {
                    decimal subtotal = detalleDTO.Cantidad * detalleDTO.PrecioUnitario;
                    total += subtotal;

                    var detalle = new PedidoDetalle
                    {
                        PedidoId = Id,
                        ProductoId = detalleDTO.ProductoId,
                        Cantidad = detalleDTO.Cantidad,
                        PrecioUnitario = detalleDTO.PrecioUnitario,
                        SubTotal = subtotal
                    };

                    newDetalles.Add(detalle);

                    var producto = await _context.Productos.FindAsync(detalleDTO.ProductoId);
                    if (producto != null)
                    {
                        producto.Stock -= detalleDTO.Cantidad;
                    }
                }

                pedido.ClienteId = request.ClienteId;
                pedido.DireccionId = request.DireccionId;
                pedido.UsuarioId = request.UsuarioId;
                pedido.Total = total;
                pedido.Estado = request.Estado ?? pedido.Estado;
                pedido.PedidoDetalles = newDetalles;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await GetByIdAsync(pedido.Id);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int Id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var pedido = await _context.Pedidos
                    .Include(p => p.PedidoDetalles)
                    .Where(p => p.Id == Id)
                    .FirstOrDefaultAsync();

                if (pedido == null)
                    return false;

                foreach (var detalle in pedido.PedidoDetalles)
                {
                    var producto = await _context.Productos.FindAsync(detalle.ProductoId);
                    if (producto != null)
                    {
                        producto.Stock += detalle.Cantidad;
                    }
                }

                _context.PedidoDetalles.RemoveRange(pedido.PedidoDetalles);

                _context.Remove(pedido);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdateEstadoAsync(int Id, string estado)
        {
            try
            {
                var pedido = await _context.Pedidos.FindAsync(Id);
                if (pedido == null)
                    return false;

                pedido.Estado = estado;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}