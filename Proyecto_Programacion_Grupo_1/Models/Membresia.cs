using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Programacion_Grupo_1.Models
{
    public class Membresia
    {
        [Key]
        public int MembresiaID { get; set; }

        public string Tipo { get; set; } // Mensual, Trimestral, Anual

        public decimal Precio { get; set; }

        public int UsuarioID { get; set; }
        [ForeignKey("UsuarioID")]
        public Usuario Usuario { get; set; }

    }
}
