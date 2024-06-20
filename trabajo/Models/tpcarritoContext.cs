using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace trabajo.Models
{
    public partial class tpcarritoContext : DbContext
    {
        public tpcarritoContext()
        {
        }

        public tpcarritoContext(DbContextOptions<tpcarritoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Carrito> Carritos { get; set; } = null!;
        public virtual DbSet<Categoria> Categoria { get; set; } = null!;
        public virtual DbSet<Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<DetalleVenta> DetalleVenta { get; set; } = null!;
        public virtual DbSet<Producto> Productos { get; set; } = null!;
        public virtual DbSet<Venta> Venta { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("server=(local); database=tpcarrito; integrated security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Carrito>(entity =>
            {
                entity.HasKey(e => e.IdCarrito)
                    .HasName("PK__CARRITO__8B4A618C4DE29236");

                entity.ToTable("CARRITO");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Carritos)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("FK__CARRITO__IdClien__3F466844");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.Carritos)
                    .HasForeignKey(d => d.IdProducto)
                    .HasConstraintName("FK__CARRITO__IdProdu__403A8C7D");
            });

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.IdCategoria)
                    .HasName("PK__Categori__A3C02A104B882A1D");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.IdCliente)
                    .HasName("PK__Cliente__D59466429E81778E");

                entity.ToTable("Cliente");

                entity.Property(e => e.Apellido)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Celular)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DetalleVenta>(entity =>
            {
                entity.HasKey(e => e.IdDetalleVenta)
                    .HasName("PK__Detalle___0C2BD9A7EB5D9D6F");

                entity.ToTable("Detalle_Venta");

                entity.Property(e => e.IdDetalleVenta).HasColumnName("IdDetalle_Venta");

                entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.DetalleVenta)
                    .HasForeignKey(d => d.IdProducto)
                    .HasConstraintName("FK__Detalle_V__IdPro__47DBAE45");

                entity.HasOne(d => d.IdVentaNavigation)
                    .WithMany(p => p.DetalleVenta)
                    .HasForeignKey(d => d.IdVenta)
                    .HasConstraintName("FK__Detalle_V__IdVen__46E78A0C");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.IdProducto)
                    .HasName("PK__Producto__098892102DE828CA");

                entity.ToTable("Producto");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCarga).HasColumnType("date");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.NombreImagen)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Nombre_Imagen");

                entity.Property(e => e.Precio)
                    .HasColumnType("decimal(10, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RutaImagen)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Ruta_Imagen");

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.IdCategoria)
                    .HasConstraintName("FK__Producto__IdCate__3B75D760");
            });

            modelBuilder.Entity<Venta>(entity =>
            {
                entity.HasKey(e => e.IdVenta)
                    .HasName("PK__Venta__BC1240BDC216FEFB");

                entity.Property(e => e.FechaVenta)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Venta")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MontoTotal)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("Monto_Total");

                entity.Property(e => e.TotalProducto).HasColumnName("Total_Producto");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Venta)
                    .HasForeignKey(d => d.IdCliente)
                    .HasConstraintName("FK__Venta__IdCliente__4316F928");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
