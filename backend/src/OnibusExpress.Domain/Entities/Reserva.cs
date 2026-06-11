using System;

namespace OnibusExpress.Domain.Entities
{
    public class Reserva
    {
        public Guid Id { get; private set; }
        public string CodigoReserva { get; private set; }
        public Guid ViagemId { get; private set; }
        public Viagem Viagem { get; private set; }
        public Guid PassageiroId { get; private set; }
        public Passageiro Passageiro { get; private set; }
        public int NumeroAssento { get; private set; }
        public ReservaStatus Status { get; private set; }
        public DateTime DataCriacao { get; private set; }

        // Construtor vazio exigido pelo Entity Framework
        protected Reserva() { }

        public Reserva(Viagem viagem, Passageiro passageiro, int numeroAssento)
        {
            if (viagem == null) throw new ArgumentException("Viagem é obrigatória.");
            if (passageiro == null) throw new ArgumentException("Passageiro é obrigatório.");
            if (numeroAssento <= 0) throw new ArgumentException("Assento inválido.");

            Id = Guid.NewGuid();
            ViagemId = viagem.Id;
            Viagem = viagem;
            PassageiroId = passageiro.Id;
            Passageiro = passageiro;
            NumeroAssento = numeroAssento;
            Status = ReservaStatus.Confirmada;
            DataCriacao = DateTime.UtcNow;
            
            GerarCodigoReserva();
        }

        private void GerarCodigoReserva()
        {
            var randomPart = Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
            CodigoReserva = $"ONI-{randomPart}";
        }

        public void Cancelar()
        {
            if (Viagem == null) throw new InvalidOperationException("Os dados da viagem não foram carregados para validação.");

            var tempoAtePartida = Viagem.DataHoraPartida - DateTime.UtcNow;
            
            if (tempoAtePartida.TotalHours < 2)
            {
                throw new InvalidOperationException("O cancelamento só é permitido até 2 horas antes da partida.");
            }

            Status = ReservaStatus.Cancelada;
        }
    }

    public enum ReservaStatus
    {
        Confirmada,
        Cancelada
    }
}