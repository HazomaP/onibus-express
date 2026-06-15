using Microsoft.EntityFrameworkCore;
using OnibusExpress.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Banco de Dados PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddScoped<OnibusExpress.Application.Services.ReservaApplicationService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adiciona a política de CORS liberando geral para testes locais
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// --- INÍCIO DA MIGRATION AUTOMÁTICA E SEED ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // 1. Puxa o contexto e cria as tabelas no banco de dados vazio
        var context = services.GetRequiredService<OnibusExpress.Infrastructure.Context.AppDbContext>();
        context.Database.Migrate();

        // 2. Usa o MESMO 'services' para chamar o Seeder logo em seguida
        OnibusExpress.Infrastructure.Context.DbSeeder.Seed(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro ao rodar as migrations na inicialização.");
    }
}
// FIM DA MIGRATION AUTOMÁTICA E SEED 

// Configura o Swagger para documentação da API
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("PermitirTudo");
app.UseAuthorization();
app.MapControllers();

app.Run();