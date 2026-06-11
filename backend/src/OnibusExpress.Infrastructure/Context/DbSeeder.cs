using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnibusExpress.Domain.Entities;

namespace OnibusExpress.Infrastructure.Context
{
    public static class DbSeeder
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            // Pega a conexão com o banco
            using var context = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

            // Se já existirem rotas, significa que o banco já foi populado, então paramos aqui.
            if (context.Rotas.Any() || context.Viagens.Any()) return;

            // 1. Cria as Rotas Base
            var rota1 = new Rota("São Paulo (Tietê)", "Rio de Janeiro (Novo Rio)", TimeSpan.FromHours(6));
            var rota2 = new Rota("Guarulhos (Aeroporto)", "Campinas (Rodoviária)", TimeSpan.FromHours(2));

            context.Rotas.AddRange(rota1, rota2);

            // 2. Cria as Viagens Futuras baseadas nessas rotas
            var viagem1 = new Viagem(rota1, DateTime.UtcNow.AddDays(2), 150.00m, 40);
            var viagem2 = new Viagem(rota1, DateTime.UtcNow.AddDays(5), 120.00m, 40);
            var viagem3 = new Viagem(rota2, DateTime.UtcNow.AddDays(1), 45.00m, 20);

            context.Viagens.AddRange(viagem1, viagem2, viagem3);
            
            // 3. Salva no banco
            context.SaveChanges();
        }
    }
}