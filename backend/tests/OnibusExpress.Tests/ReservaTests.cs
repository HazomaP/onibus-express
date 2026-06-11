using System;
using OnibusExpress.Domain.Entities;
using OnibusExpress.Domain.ValueObjects;
using Xunit;

namespace OnibusExpress.Tests
{
    public class ReservaTests
    {
        private Viagem CriarViagem(DateTime dataHoraPartida)
        {
            var rota = new Rota("São Paulo", "Rio de Janeiro", TimeSpan.FromHours(6));
            return new Viagem(rota, dataHoraPartida, 150.0m, 40);
        }

        private Passageiro CriarPassageiro()
{
    var cpf = new Cpf("52998224725");
    // Adicionada a data de nascimento para bater com o novo construtor
    return new Passageiro("João Silva", cpf, "joao@email.com", new DateTime(1990, 1, 1));
}


        [Fact]
        public void Deve_Permitir_Cancelamento_Com_Mais_De_Duas_Horas_De_Antecedencia()
        {
            // Arrange: Viagem para daqui a 5 horas
            var viagemFutura = CriarViagem(DateTime.UtcNow.AddHours(5));
            var passageiro = CriarPassageiro();
            var reserva = new Reserva(viagemFutura, passageiro, 15);

            // Act
            reserva.Cancelar();

            // Assert
            Assert.Equal(ReservaStatus.Cancelada, reserva.Status);
        }

        [Fact]
        public void Nao_Deve_Permitir_Cancelamento_Com_Menos_De_Duas_Horas_De_Antecedencia()
        {
            // Arrange: Viagem para daqui a 1 hora
            var viagemProxima = CriarViagem(DateTime.UtcNow.AddHours(1));
            var passageiro = CriarPassageiro();
            var reserva = new Reserva(viagemProxima, passageiro, 15);

            // Act & Assert
            var excecao = Assert.Throws<InvalidOperationException>(() => reserva.Cancelar());
            Assert.Equal("O cancelamento só é permitido até 2 horas antes da partida.", excecao.Message);
        }
    }
}