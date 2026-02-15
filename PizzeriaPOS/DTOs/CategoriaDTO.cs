namespace PizzeriaPOS.DTOs
{
    //DTO para crear/actualizar la Categoria
    public class CategoriaCreateUpdateDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string? Descripcion { get; set; }

        public bool? Activa { get; set; }
    }
    //DTO para visualizar la Categoria
    public class CategoriaDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string? Descripcion { get; set; }

        public bool? Activa { get; set; }
    }

}
