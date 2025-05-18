using Microsoft.EntityFrameworkCore;
using ProyectoCentinela.Models;

namespace ProyectoCentinela.Data
{
    /// <summary>
    /// DbContext principal que gestiona la conexión y acceso a las tablas.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Referencias a las tablas de la base de datos
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Sesion> Sesiones { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Leaderboard> Leaderboards { get; set; }

        /// <summary>
        /// Configuración de las relaciones y nombres de tabla.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de la tabla Usuario
            modelBuilder.Entity<Usuario>()
                .ToTable("usuario")
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolId);

            // Configuración de la tabla Sesion con Cascade Delete
            modelBuilder.Entity<Sesion>()
                .ToTable("sesion")
                .HasOne(s => s.Usuario)
                .WithMany(u => u.Sesiones)
                .HasForeignKey(s => s.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de la tabla Leaderboard con Cascade Delete
            modelBuilder.Entity<Leaderboard>()
                .ToTable("leaderboard")
                .HasOne(l => l.Usuario)
                .WithMany(u => u.Leaderboards)
                .HasForeignKey(l => l.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de la tabla Rol
            modelBuilder.Entity<Rol>()
                .ToTable("rol");
        }
    }
}