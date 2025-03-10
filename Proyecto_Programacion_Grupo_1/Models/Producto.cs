using System.ComponentModel.DataAnnotations;

namespace Proyecto_Programacion_Grupo_1.Models
{
    public class Producto
    {
        [Key]
        public int ProductoID { get; set; }

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; }

        [Required]
        public decimal Precio { get; set; }

        public int Stock { get; set; }
    }
}
