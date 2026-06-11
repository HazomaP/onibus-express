using System;
using System.Linq;

namespace OnibusExpress.Domain.ValueObjects
{
    public class Cpf
    {
        public string Numero { get; private set; }

        protected Cpf() { }

        public Cpf(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
                throw new ArgumentException("O CPF não pode ser vazio.");

            var cpfLimpo = new string(numero.Where(char.IsDigit).ToArray());

            if (!Validar(cpfLimpo))
                throw new ArgumentException("CPF inválido.");

            Numero = cpfLimpo;
        }

        private static bool Validar(string cpf)
        {
            if (cpf.Length != 11) return false;
            if (cpf.Distinct().Count() == 1) return false;

            var multiplicadores1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicadores2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            var tempCpf = cpf.Substring(0, 9);
            var soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicadores1[i];

            var resto = soma % 11;
            var digito1 = resto < 2 ? 0 : 11 - resto;

            tempCpf += digito1;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicadores2[i];

            resto = soma % 11;
            var digito2 = resto < 2 ? 0 : 11 - resto;

            return cpf.EndsWith(digito1.ToString() + digito2.ToString());
        }
    }
}