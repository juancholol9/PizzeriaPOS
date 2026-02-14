using System;
using System.Collections.Generic;

namespace PizzeriaPOS.Models;

public partial class Categoria
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool? Activa { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
