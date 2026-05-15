using Microsoft.EntityFrameworkCore;
using PCPProyect.Models;

namespace PCPProyect.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        // TABLAS
        public DbSet<Movcabe1ot01> Movcabe1ot01 { get; set; }
        public DbSet<Movdete1ot01> Movdete1ot01 { get; set; }
        public DbSet<MovHis00> MovHis00 { get; set; }
        public DbSet<Articulo> Articulo { get; set; }
        public DbSet<VWProyeccionGrid> VWProyeccionGrid { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MovHis00>()
        .ToTable(tb => tb.UseSqlOutputClause(false));

            // =========================
            // CABECERA
            // =========================
            modelBuilder.Entity<Movcabe1ot01>()
                .HasKey(x => new { x.NumDoc, x.CodDoc });

            // =========================
            // DETALLE
            // =========================
            modelBuilder.Entity<Movdete1ot01>()
                .HasKey(x => new { x.CodDoc, x.NumDoc, x.NumIte });

            // =========================
            // PROYECCION (HISTORIAL)
            // =========================
            modelBuilder.Entity<MovHis00>()
                .HasKey(x => new { x.CodEmp, x.CodDoc, x.NumDoc, x.FechaHis });

            // =========================
            // RELACIONES (opcional pero recomendado)
            // =========================

            modelBuilder.Entity<Movdete1ot01>()
                .HasOne<Movcabe1ot01>()
                .WithMany()
                .HasForeignKey(x => new { x.CodDoc, x.NumDoc });

            //  MovHis00 no tiene FK formal, lo usaremos manualmente en queries


            modelBuilder.Entity<Articulo>()
    .HasKey(x => new
    {
        x.CodEmp,
        x.CodSubAlm,
        x.CodArt
    });
        }
    }
}
