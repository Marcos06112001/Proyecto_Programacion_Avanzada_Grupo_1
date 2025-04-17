using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Programacion_Grupo_1.Models
{
    public class CarritoItem
    {
        [Key]
        public int CarritoItemId { get; set; }

        public int ProductoID { get; set; }
        public Producto? Producto { get; set; }
        public int MembresiaID { get; set; }
        public Membresia? Membresia { get; set; }

        public int Cantidad { get; set; }

        [NotMapped]
        public decimal Subtotal => Producto?.Precio * Cantidad ?? 0;
    }
}
