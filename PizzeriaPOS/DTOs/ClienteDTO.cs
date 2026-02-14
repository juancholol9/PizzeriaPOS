namespace PizzeriaPOS.DTOs
{
    public class ClienteCreateUpdateDTO
    {
        public string Nombre { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }

    }

    public class ClienteDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
    }
}
