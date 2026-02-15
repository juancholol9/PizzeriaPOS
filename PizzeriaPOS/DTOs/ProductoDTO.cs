namespace PizzeriaPOS.DTOs
{
    public class ProductoCreateUpdateDTO
    {
        public int CategoriaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool? Activo { get; set; }
    }

    public class ProductoDTO
    {
        public int Id { get; set; }
        public int CategoriaId { get; set; }
        public string Categoria { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }

    public class ProductoDetailDTO : ProductoDTO
    {
        public CategoriaDTO? Categoria { get; set; }
    }
}
