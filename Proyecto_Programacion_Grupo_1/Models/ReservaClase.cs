using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Programacion_Grupo_1.Models
{
    public class ReservaClase
    {
        [Key]
        public int ReservaID { get; set; }

        public int UsuarioID { get; set; }
        public int ClaseID { get; set; }

        [ForeignKey("UsuarioID")]
        public Usuario Usuario { get; set; }

        [ForeignKey("ClaseID")]
        public Clase Clase { get; set; }

        public DateTime FechaReserva { get; set; }

    }
}
