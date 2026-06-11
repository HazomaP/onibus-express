using System;
using OnibusExpress.Domain.ValueObjects;

namespace OnibusExpress.Domain.Entities
{
    public class Passageiro
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public Cpf Cpf { get; private set; }
        public string Email { get; private set; }
        public DateTime DataNascimento { get; private set; }

        protected Passageiro() { }

        public Passageiro(string nome, Cpf cpf, string email, DateTime dataNascimento)
        {
            if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("O nome é obrigatório.");
            if (cpf == null) throw new ArgumentNullException(nameof(cpf));
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("O e-mail é obrigatório.");
            if (dataNascimento >= DateTime.UtcNow) throw new ArgumentException("A data de nascimento não pode ser no futuro.");

            Id = Guid.NewGuid();
            Nome = nome;
            Cpf = cpf;
            Email = email;
            DataNascimento = dataNascimento;
        }
    }
}