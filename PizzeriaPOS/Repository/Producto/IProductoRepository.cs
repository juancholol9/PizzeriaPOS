using PizzeriaPOS.DTOs;

namespace PizzeriaPOS.Repository
{
    public interface IProductoRepository
    {
        Task<IEnumerable<ProductoDTO>> GetAllAsync();
        Task<IEnumerable<ProductoDTO>> GetByCategoriaIdAsync(int categoriaId);
        Task<ProductoDTO?> GetByIdAsync(int Id);
        Task<ProductoDTO?> CreateAsync(ProductoCreateUpdateDTO request);
        Task<ProductoDTO?> UpdateAsync(ProductoCreateUpdateDTO request, int Id);
        Task<bool> DeleteAsync(int Id);
    }
}