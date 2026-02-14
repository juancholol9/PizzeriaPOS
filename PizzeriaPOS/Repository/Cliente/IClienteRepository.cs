using PizzeriaPOS.DTOs;

namespace PizzeriaPOS.Repository
{
    public interface IClienteRepository
    {
        Task<IEnumerable<ClienteDTO>> GetAllAsync();
        Task<ClienteDTO?> GetByIdAsync(int Id);
        Task<ClienteDTO?> CreateAsync(ClienteCreateUpdateDTO request);
        Task<ClienteDTO?> UpdateAsync(ClienteCreateUpdateDTO request, int Id);
        Task<bool> DeleteAsync(int Id);
    }



}
