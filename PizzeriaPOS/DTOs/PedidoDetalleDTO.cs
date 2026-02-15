namespace PizzeriaPOS.DTOs
{
    public class PedidoDetalleCreateUpdateDTO
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
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

    public class PedidoDetalleDetailDTO : PedidoDetalleDTO
    {
        public ProductoDTO? Producto { get; set; }
    }
}