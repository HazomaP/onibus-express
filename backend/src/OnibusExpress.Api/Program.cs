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

var app = builder.Build();

// Executa o Seeder para popular o banco de dados vazio
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    OnibusExpress.Infrastructure.Context.DbSeeder.Seed(services);
}

// Configura o Swagger para documentação da API
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();