using System;

namespace OnibusExpress.Domain.Entities
{
    public class Viagem
    {
        public Guid Id { get; private set; }
        public Guid RotaId { get; private set; }
        public Rota Rota { get; private set; }
        public DateTime DataHoraPartida { get; private set; }
        public decimal PrecoBase { get; private set; }
        public int TotalAssentos { get; private set; }

        protected Viagem() { }

        public Viagem(Rota rota, DateTime dataHoraPartida, decimal precoBase, int totalAssentos)
        {
            if (precoBase < 0) throw new ArgumentException("O preço base não pode ser negativo.");
            if (totalAssentos <= 0) throw new ArgumentException("A viagem deve ter pelo menos um assento.");

            Id = Guid.NewGuid();
            Rota = rota ?? throw new ArgumentNullException(nameof(rota));
            RotaId = rota.Id;
            DataHoraPartida = dataHoraPartida;
            PrecoBase = precoBase;
            TotalAssentos = totalAssentos;
        }

        public bool IsPassada() => DataHoraPartida <= DateTime.UtcNow;
    }
}