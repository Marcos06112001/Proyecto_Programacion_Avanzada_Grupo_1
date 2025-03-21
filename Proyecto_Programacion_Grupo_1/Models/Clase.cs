using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Programacion_Grupo_1.Models
{
    public class Clase
    {
        [Key]
        public int ClaseID { get; set; }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaHora { get; set; }
        public int AcademiaID { get; set; }

        [ForeignKey("AcademiaID")]
        public Academia? Academia { get; set; }
    }
}
