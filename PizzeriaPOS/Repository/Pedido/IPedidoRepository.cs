using PizzeriaPOS.DTOs;

namespace PizzeriaPOS.Repository
{
    public interface IPedidoRepository
    {
        Task<IEnumerable<PedidoDTO>> GetAllAsync();
        Task<IEnumerable<PedidoDTO>> GetByClienteIdAsync(int clienteId);
        Task<PedidoDetailDTO?> GetByIdAsync(int Id);
        Task<PedidoDetailDTO?> CreateAsync(PedidoCreateUpdateDTO request);
        Task<PedidoDetailDTO?> UpdateAsync(PedidoCreateUpdateDTO request, int Id);
        Task<bool> DeleteAsync(int Id);
        Task<bool> UpdateEstadoAsync(int Id, string estado);
    }
}