using Microsoft.EntityFrameworkCore;

namespace Proyecto_Programacion_Grupo_1.Models
{
    public class ProyectoContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Membresia> Membresias { get; set; }
        public DbSet<Clase> Clases { get; set; }
        public DbSet<Academia> Academias { get; set; }
        public DbSet<ReservaClase> ReservasClase { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<CarritoItem> Carrito { get; set; }
        public DbSet<CarritoCompra> Carritoc { get; set; }
        public DbSet<Pago> Pagos { get; set; }

        public ProyectoContext(DbContextOptions<ProyectoContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set precision and scale for Monto in Pago
            modelBuilder.Entity<Pago>()
                .Property(p => p.Monto)
                .HasColumnType("decimal(18,2)");

            // Set precision and scale for Precio in Producto
            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(18,2)");

            // Set precision and scale for Precio in Membresia
            modelBuilder.Entity<Membresia>()
                .Property(m => m.Precio)
                .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
