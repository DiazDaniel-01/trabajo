using System;
using System.Collections.Generic;

namespace trabajo.Models
{
    public partial class Producto
    {
        public Producto()
        {
            Carritos = new HashSet<Carrito>();
            DetalleVenta = new HashSet<DetalleVenta>();
        }

        public int IdProducto { get; set; }
        public int? IdCategoria { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal? Precio { get; set; }
        public string? RutaImagen { get; set; }
        public string? NombreImagen { get; set; }
        public DateTime? FechaCarga { get; set; }
        public virtual Categoria? IdCategoriaNavigation { get; set; }
        public virtual ICollection<Carrito> Carritos { get; set; }
        public virtual ICollection<DetalleVenta> DetalleVenta { get; set; }
    }
}
