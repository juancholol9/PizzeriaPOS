using System;
using System.Collections.Generic;

namespace PizzeriaPOS.Models;

public partial class Pedido
{
    public int Id { get; set; }

    public int ClienteId { get; set; }

    public int DireccionId { get; set; }

    public int UsuarioId { get; set; }

    public DateTime? Fecha { get; set; }

    public decimal Total { get; set; }

    public string? Estado { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual Direccion Direccion { get; set; } = null!;

    public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; } = new List<PedidoDetalle>();

    public virtual Usuario Usuario { get; set; } = null!;
}
