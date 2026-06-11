using System;

namespace OnibusExpress.Domain.Entities
{
    public class Rota
    {
        public Guid Id { get; private set; }
        public string Origem { get; private set; }
        public string Destino { get; private set; }
        public TimeSpan DuracaoEstimada { get; private set; }

        protected Rota() { }

        public Rota(string origem, string destino, TimeSpan duracaoEstimada)
        {
            if (string.IsNullOrWhiteSpace(origem)) throw new ArgumentException("A origem é obrigatória.");
            if (string.IsNullOrWhiteSpace(destino)) throw new ArgumentException("O destino é obrigatório.");
            if (duracaoEstimada <= TimeSpan.Zero) throw new ArgumentException("A duração estimada deve ser maior que zero.");

            Id = Guid.NewGuid();
            Origem = origem;
            Destino = destino;
            DuracaoEstimada = duracaoEstimada;
        }
    }
}