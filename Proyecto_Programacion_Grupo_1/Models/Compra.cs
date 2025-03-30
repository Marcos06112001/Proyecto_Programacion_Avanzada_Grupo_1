using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Programacion_Grupo_1.Models
{
    public class Compra
    {
        [Key]
        public int CompraID { get; set; }

        public int UsuarioID { get; set; }
        public int ProductoID { get; set; }
        public int Cantidad { get; set; }

        public DateTime FechaCompra { get; set; }

        [ForeignKey("UsuarioID")]
        public Usuario? Usuario { get; set; }

        [ForeignKey("ProductoID")]
        public Producto? Producto { get; set; }

    }
}
