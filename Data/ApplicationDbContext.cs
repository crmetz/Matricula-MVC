using Matriculas.Models;
using Microsoft.EntityFrameworkCore;

namespace Matriculas.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Curso> Cursos => Set<Curso>();
        public DbSet<Matricula> Matriculas => Set<Matricula>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Matricula>(entity =>
            {
                entity.HasIndex(m => new { m.Ano, m.CursoId }).IsUnique();

                entity.HasOne(m => m.Curso)
                      .WithMany(c => c.Matriculas)
                      .HasForeignKey(m => m.CursoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
