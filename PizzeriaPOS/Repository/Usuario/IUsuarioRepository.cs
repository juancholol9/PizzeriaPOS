using PizzeriaPOS.DTOs;

namespace PizzeriaPOS.Repository
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<UsuarioDTO>> GetAllAsync();
        Task<UsuarioDTO?> GetByIdAsync(int Id);
        Task<UsuarioDTO?> GetByNombreUsuarioAsync(string nombreUsuario);
        Task<UsuarioDTO?> CreateAsync(UsuarioCreateUpdateDTO request);
        Task<UsuarioDTO?> UpdateAsync(UsuarioCreateUpdateDTO request, int Id);
        Task<bool> DeleteAsync(int Id);
        Task<UsuarioDTO?> ValidateLoginAsync(UsuarioLoginDTO request);
    }
}