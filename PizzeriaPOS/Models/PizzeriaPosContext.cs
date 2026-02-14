using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PizzeriaPOS.Models;

public partial class PizzeriaPosContext : DbContext
{
    public PizzeriaPosContext()
    {
    }

    public PizzeriaPosContext(DbContextOptions<PizzeriaPosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Direccion> Direccions { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<PedidoDetalle> PedidoDetalles { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07AC4A4E0F");

            entity.Property(e => e.Activa).HasDefaultValue(true);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cliente__3214EC071AE2BE91");

            entity.ToTable("Cliente");

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Direccion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Direccio__3214EC0732B497D8");

            entity.ToTable("Direccion");

            entity.Property(e => e.Activa).HasDefaultValue(true);
            entity.Property(e => e.Calle)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Ciudad)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Referencia)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Cliente).WithMany(p => p.Direccions)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("FK_Direccion_Cliente");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pedido__3214EC071B058BB7");

            entity.ToTable("Pedido");

            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedido_Cliente");

            entity.HasOne(d => d.Direccion).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.DireccionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedido_Direccion");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedido_Usuario");
        });

        modelBuilder.Entity<PedidoDetalle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PedidoDe__3214EC07DC879DF5");

            entity.ToTable("PedidoDetalle");

            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Pedido).WithMany(p => p.PedidoDetalles)
                .HasForeignKey(d => d.PedidoId)
                .HasConstraintName("FK_PedidoDetalle_Pedido");

            entity.HasOne(d => d.Producto).WithMany(p => p.PedidoDetalles)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PedidoDetalle_Producto");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producto__3214EC072EDCF31A");

            entity.ToTable("Producto");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Productos)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_Categoria");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC07A4FCD4F3");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.NombreUsuario, "UQ__Usuario__6B0F5AE0469BDFAB").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Usuario__A9D10534A5F965FE").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Empleado");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
