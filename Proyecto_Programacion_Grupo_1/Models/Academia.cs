using System.ComponentModel.DataAnnotations;

namespace Proyecto_Programacion_Grupo_1.Models
{
    public class Academia
    {
        [Key]
        public int AcademiaID { get; set; }

        public string Nombre { get; set; }
        public string Ubicacion { get; set; }
        public string Horario { get; set; }

        public ICollection<Clase> Clases { get; set; }
    }
}
