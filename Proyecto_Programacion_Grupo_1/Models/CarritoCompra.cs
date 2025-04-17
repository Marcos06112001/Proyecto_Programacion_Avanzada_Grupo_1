using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Programacion_Grupo_1.Models
{
    public class CarritoCompra
    {
        public int CarritoCompraId { get; set; }
        public List<CarritoItem> Items { get; set; } = new List<CarritoItem>();

        [NotMapped]
        public decimal Total => Items.Sum(i => i.Subtotal);
    }
}
