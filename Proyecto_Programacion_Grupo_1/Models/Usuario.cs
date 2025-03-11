using System.ComponentModel.DataAnnotations;

namespace Proyecto_Programacion_Grupo_1.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioID { get; set; }

        [Required]
        [StringLength(100)]
        public string? Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string? Correo { get; set; }

        [Required]
        public string? Contraseña { get; set; }

        public string? Rol { get; set; } // "Admin" o "Usuario"

        public ICollection<ReservaClase>? Reservas { get; set; }
        public ICollection<Compra>? Compras { get; set; }
        public ICollection<Pago>? Pagos { get; set; }

    }
}
