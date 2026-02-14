using System;
using System.Collections.Generic;

namespace PizzeriaPOS.Models;

public partial class Direccion
{
    public int Id { get; set; }

    public int ClienteId { get; set; }

    public string Calle { get; set; } = null!;

    public string Ciudad { get; set; } = null!;

    public string? Referencia { get; set; }

    public bool? Activa { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
