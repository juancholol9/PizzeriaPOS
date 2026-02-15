namespace PizzeriaPOS.DTOs
{
    public class PedidoCreateUpdateDTO
    {
        public int ClienteId { get; set; }
        public int DireccionId { get; set; }
        public int UsuarioId { get; set; }
        public string? Estado { get; set; }
        public List<PedidoDetalleCreateUpdateDTO> Detalles { get; set; } = new();
        // Total will be calculated from Detalles
    }

    public class PedidoDTO
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int DireccionId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime? Fecha { get; set; }
        public decimal Total { get; set; }
        public string? Estado { get; set; }
    }

    // Optional: For detailed view with all relationships
    public class PedidoDetailDTO : PedidoDTO
    {
        public ClienteDTO? Cliente { get; set; }
        public DireccionDTO? Direccion { get; set; }
        public UsuarioDTO? Usuario { get; set; }
        public List<PedidoDetalleDetailDTO> Detalles { get; set; } = new();
    }
}