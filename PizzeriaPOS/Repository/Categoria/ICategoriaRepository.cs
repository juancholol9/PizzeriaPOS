using PizzeriaPOS.DTOs;

namespace PizzeriaPOS.Repository
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<CategoriaDTO>> GetAllAsync();
        Task<CategoriaDTO?> GetByIdAsync(int Id);
        Task<CategoriaDTO?> CreateAsync(CategoriaCreateUpdateDTO request);
        Task<CategoriaDTO?> UpdateAsync(CategoriaCreateUpdateDTO request, int Id);
        Task<bool> DeleteAsync(int Id);
    }
}