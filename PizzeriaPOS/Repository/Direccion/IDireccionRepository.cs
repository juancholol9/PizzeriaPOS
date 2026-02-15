using PizzeriaPOS.DTOs;

namespace PizzeriaPOS.Repository
{
    public interface IDireccionRepository
    {
        Task<IEnumerable<DireccionDTO>> GetAllAsync();
        Task<IEnumerable<DireccionDTO>> GetByClienteIdAsync(int clienteId);
        Task<DireccionDTO?> GetByIdAsync(int Id);
        Task<DireccionDTO?> CreateAsync(DireccionCreateUpdateDTO request);
        Task<DireccionDTO?> UpdateAsync(DireccionCreateUpdateDTO request, int Id);
        Task<bool> DeleteAsync(int Id);
    }
}