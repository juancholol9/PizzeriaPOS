namespace PizzeriaPOS.DTOs
{
    //DTO para crear/actualizar el Cliente
    public class ClienteCreateUpdateDTO
    {
        public string Nombre { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }

    }

    //DTO para visualizar el Cliente
    public class ClienteDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
    }
}
