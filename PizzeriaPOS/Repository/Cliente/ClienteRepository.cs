using Microsoft.EntityFrameworkCore;
using PizzeriaPOS.DTOs;
using PizzeriaPOS.Models;

namespace PizzeriaPOS.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly PizzeriaPosContext _context;

        public ClienteRepository(PizzeriaPosContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClienteDTO>> GetAllAsync()
        {
            try
            {
                var clientes = await _context.Clientes.ToListAsync();

                var response = clientes.Select(c => new ClienteDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Telefono = c.Telefono,
                    Email = c.Email
                });

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ClienteDTO?> GetByIdAsync(int Id)
        {

            try
            {
                var cliente = await _context.Clientes.Where(c => c.Id == Id).FirstOrDefaultAsync();

                var response = new ClienteDTO
                {
                    Id = cliente.Id,
                    Nombre = cliente.Nombre,
                    Telefono = cliente.Telefono,
                    Email = cliente.Email,
                };

                return response;
            }

            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ClienteDTO?> CreateAsync(ClienteCreateUpdateDTO request)
        {
            try
            {
                if (await EmailExist(request.Email))
                {
                    throw new Exception("Este email ya existe");
                }

                var cliente = new Cliente
                {
                    Nombre = request.Nombre,
                    Email = request.Email,
                    Telefono = request.Telefono,
                };

                await _context.Clientes.AddAsync(cliente);
                await _context.SaveChangesAsync();

                return await GetByIdAsync(cliente.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ClienteDTO?> UpdateAsync(ClienteCreateUpdateDTO request, int Id)
        {
            try
            {
                var cliente = await _context.Clientes.FindAsync(Id);
                if (cliente == null)
                    return null;


                if (await EmailExist(request.Email, Id))
                {
                    throw new Exception("Este email ya existe");
                }

                cliente.Nombre = request.Nombre;
                cliente.Email = request.Email;
                cliente.Telefono = request.Telefono;

                await _context.SaveChangesAsync();

                return await GetByIdAsync(cliente.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int Id)
        {
            var cliente = await _context.Clientes.FindAsync(Id);
            if (cliente == null)
                return false;

            _context.Remove(cliente);
            await _context.SaveChangesAsync();
            return true;
        }



        private async Task<bool> EmailExist(string email, int? id = null)
        {
            try
            {
                var query = _context.Clientes.Where(c => c.Email == email);

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
