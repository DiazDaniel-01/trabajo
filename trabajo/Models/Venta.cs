using System;
using System.Collections.Generic;

namespace trabajo.Models
{
    public partial class Venta
    {
        public Venta()
        {
            DetalleVenta = new HashSet<DetalleVenta>();
        }

        public int IdVenta { get; set; }
        public int? IdCliente { get; set; }
        public int? TotalProducto { get; set; }
        public decimal? MontoTotal { get; set; }
        public DateTime? FechaVenta { get; set; }

        public virtual Cliente? IdClienteNavigation { get; set; }
        public virtual ICollection<DetalleVenta> DetalleVenta { get; set; }
    }
}
