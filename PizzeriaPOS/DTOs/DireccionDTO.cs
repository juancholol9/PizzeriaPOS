namespace PizzeriaPOS.DTOs
{
    public class DireccionCreateUpdateDTO
    {
        public int ClienteId { get; set; }
        public string Calle { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string? Referencia { get; set; }
        public bool? Activa { get; set; }
    }

    public class DireccionDTO
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Cliente { get; set; }
        public string Calle { get; set; } = null!;
        public string Ciudad { get; set; } = null!;
        public string? Referencia { get; set; }
        public bool? Activa { get; set; }
    }

    public class DireccionDetailDTO : DireccionDTO
    {
        public ClienteDTO? Cliente { get; set; }
    }
}
