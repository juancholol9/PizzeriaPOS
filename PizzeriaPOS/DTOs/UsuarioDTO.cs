namespace PizzeriaPOS.DTOs
{
    public class UsuarioCreateUpdateDTO
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // Plain password (will be hashed)
        public string Rol { get; set; } = "Empleado";
        public bool? Activo { get; set; }
    }

    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = null!;
        public string Email { get; set; } = null!;
        // NOTE: PasswordHash is intentionally NOT included for security
        public string Rol { get; set; } = null!;
        public bool? Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }

    // For login requests
    public class UsuarioLoginDTO
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
