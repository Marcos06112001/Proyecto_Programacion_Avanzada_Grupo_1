using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Programacion_Grupo_1.Models
{
    public class CarritoItem
    {
        [Key]
        public int CarritoItemId { get; set; }

        public int? ProductoID { get; set; }
        public Producto? Producto { get; set; }

        public int? MembresiaID { get; set; }
        public Membresia? Membresia { get; set; }

        public int Cantidad { get; set; }

        [NotMapped]
        public decimal Subtotal
        {
            get
            {
                if (Producto != null)
                    return Producto.Precio * Cantidad;
                else if (Membresia != null)
                    return Membresia.Precio * Cantidad;
                else
                    return 0;
            }
        }
    }
}
