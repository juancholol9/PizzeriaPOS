namespace PizzeriaPOS.DTOs
{
    public class PedidoDetalleCreateUpdateDTO
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        // SubTotal can be calculated: Cantidad * PrecioUnitario
    }

    public class PedidoDetalleDTO
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal SubTotal { get; set; }
    }

    // Optional: For when you need to include Producto info
    public class PedidoDetalleDetailDTO : PedidoDetalleDTO
    {
        public ProductoDTO? Producto { get; set; }
    }
}