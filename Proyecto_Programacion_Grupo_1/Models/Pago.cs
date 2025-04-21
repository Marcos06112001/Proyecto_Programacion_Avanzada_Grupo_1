using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Programacion_Grupo_1.Models
{
    public class Pago
    {
        [Key]
        public int PagoID { get; set; }

        public int UsuarioID { get; set; }

        [Required]
        public decimal Monto { get; set; }

        public string MetodoPago { get; set; } // Sinpe, PayPal, Transferencia

        public DateTime FechaPago { get; set; }

        [ForeignKey("UsuarioID")]
        public Usuario? Usuario { get; set; }
    }
}
