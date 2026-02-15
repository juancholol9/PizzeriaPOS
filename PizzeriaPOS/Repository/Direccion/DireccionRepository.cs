using Microsoft.EntityFrameworkCore;
using PizzeriaPOS.DTOs;
using PizzeriaPOS.Models;

namespace PizzeriaPOS.Repository
{
    public class DireccionRepository : IDireccionRepository
    {
        private readonly PizzeriaPosContext _context;

        public DireccionRepository(PizzeriaPosContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DireccionDTO>> GetAllAsync()
        {
            try
            {
                var direcciones = from d in _context.Direccions
                                  from c in _context.Clientes.Where(c => c.Id == d.ClienteId).DefaultIfEmpty()
                                  select new DireccionDTO
                                  {
                                      Id = d.Id,
                                      ClienteId = d.ClienteId,
                                      Cliente = c.Nombre,
                                      Calle = d.Calle,
                                      Ciudad = d.Ciudad,
                                      Referencia = d.Referencia,
                                      Activa = d.Activa
                                  };

                return await direcciones.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<DireccionDTO>> GetByClienteIdAsync(int clienteId)
        {
            try
            {
                var direcciones = from d in _context.Direccions.Where(d => d.ClienteId == clienteId)
                                  from c in _context.Clientes.Where(c => c.Id == d.ClienteId).DefaultIfEmpty()
                                  select new DireccionDTO
                                  {
                                      Id = d.Id,
                                      ClienteId = d.ClienteId,
                                      Cliente = c.Nombre,
                                      Calle = d.Calle,
                                      Ciudad = d.Ciudad,
                                      Referencia = d.Referencia,
                                      Activa = d.Activa
                                  };

                return await direcciones.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DireccionDTO?> GetByIdAsync(int Id)
        {
            try
            {
                var direcciones = from d in _context.Direccions.Where(d => d.Id == Id)
                                  from c in _context.Clientes.Where(c => c.Id == d.ClienteId).DefaultIfEmpty()
                                  select new DireccionDTO
                                  {
                                      Id = d.Id,
                                      ClienteId = d.ClienteId,
                                      Cliente = c.Nombre,
                                      Calle = d.Calle,
                                      Ciudad = d.Ciudad,
                                      Referencia = d.Referencia,
                                      Activa = d.Activa
                                  };

                return await direcciones.FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DireccionDTO?> CreateAsync(DireccionCreateUpdateDTO request)
        {
            try
            {
                var clienteExists = await _context.Clientes.AnyAsync(c => c.Id == request.ClienteId);
                if (!clienteExists)
                {
                    throw new Exception("El cliente especificado no existe");
                }

                var direccion = new Direccion
                {
                    ClienteId = request.ClienteId,
                    Calle = request.Calle,
                    Ciudad = request.Ciudad,
                    Referencia = request.Referencia,
                    Activa = request.Activa ?? true
                };

                await _context.Direccions.AddAsync(direccion);
                await _context.SaveChangesAsync();

                return await GetByIdAsync(direccion.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DireccionDTO?> UpdateAsync(DireccionCreateUpdateDTO request, int Id)
        {
            try
            {
                var direccion = await _context.Direccions.FindAsync(Id);
                if (direccion == null)
                    return null;

                var clienteExists = await _context.Clientes.AnyAsync(c => c.Id == request.ClienteId);
                if (!clienteExists)
                {
                    throw new Exception("El cliente especificado no existe");
                }

                direccion.ClienteId = request.ClienteId;
                direccion.Calle = request.Calle;
                direccion.Ciudad = request.Ciudad;
                direccion.Referencia = request.Referencia;
                direccion.Activa = request.Activa;

                await _context.SaveChangesAsync();

                return await GetByIdAsync(direccion.Id);
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
                var direccion = await _context.Direccions.FindAsync(Id);
                if (direccion == null)
                    return false;

                var hasPedidos = await _context.Pedidos.AnyAsync(p => p.DireccionId == Id);
                if (hasPedidos)
                {
                    throw new Exception("No se puede eliminar la dirección porque tiene pedidos asociados");
                }

                _context.Remove(direccion);
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