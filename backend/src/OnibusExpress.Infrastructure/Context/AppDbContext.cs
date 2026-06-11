using Microsoft.EntityFrameworkCore;
using OnibusExpress.Domain.Entities;

namespace OnibusExpress.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Rota> Rotas { get; set; }
        public DbSet<Viagem> Viagens { get; set; }
        public DbSet<Passageiro> Passageiros { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mapeamento da Reserva
            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.CodigoReserva).IsUnique(); // Garante código único no banco
                entity.Property(e => e.CodigoReserva).HasMaxLength(20).IsRequired();
            });

            // Mapeamento do Passageiro e do Value Object (CPF)
            modelBuilder.Entity<Passageiro>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                // Ensina o EF Core a salvar o Value Object Cpf na mesma tabela do Passageiro
                entity.OwnsOne(e => e.Cpf, cpf =>
                {
                    cpf.Property(c => c.Numero)
                       .HasColumnName("Cpf")
                       .HasMaxLength(11)
                       .IsRequired();
                });
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}