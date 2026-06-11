using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnibusExpress.Domain.Entities;
using OnibusExpress.Domain.ValueObjects;
using OnibusExpress.Infrastructure.Context;

namespace OnibusExpress.Application.Services
{
    public class ReservaApplicationService
    {
        private readonly AppDbContext _context;

        public ReservaApplicationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Reserva> CriarReservaAsync(Guid viagemId, string nome, string cpfInput, string email, DateTime dataNascimento, int numeroAssento)
        {
            var cpf = new Cpf(cpfInput);

            var viagem = await _context.Viagens.FindAsync(viagemId);
            if (viagem == null) throw new Exception("Viagem não encontrada.");

            if (viagem.IsPassada()) 
                throw new InvalidOperationException("Não é possível reservar passagens para uma viagem que já ocorreu.");

            if (numeroAssento <= 0 || numeroAssento > viagem.TotalAssentos)
                throw new ArgumentException("Número de assento inválido para esta viagem.");

            var assentoOcupado = await _context.Reservas
                .AnyAsync(r => r.ViagemId == viagemId && r.NumeroAssento == numeroAssento && r.Status == ReservaStatus.Confirmada);

            if (assentoOcupado)
                throw new InvalidOperationException("Este assento já está ocupado.");

            var passageiro = await _context.Passageiros
                .FirstOrDefaultAsync(p => p.Cpf.Numero == cpf.Numero) 
                ?? new Passageiro(nome, cpf, email, dataNascimento);

            if (passageiro.Id == Guid.Empty) _context.Passageiros.Add(passageiro);

            var reserva = new Reserva(viagem, passageiro, numeroAssento);

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return reserva;
        }
    }
}