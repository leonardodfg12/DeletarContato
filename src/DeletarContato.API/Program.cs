using System.Diagnostics.CodeAnalysis;
using DeletarContato.Application.Services;
using DeletarContato.Infrastructure.Config;
using DeletarContato.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeletarContato.API;

public partial class Program
{
    [ExcludeFromCodeCoverage]
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure logging
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        // Configura a conexão com o banco de dados com EnableRetryOnFailure
        builder.Services.AddDbContext<ContactZoneDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

        // Configurar MassTransit via Infrastructure
        builder.Services.AddMassTransitConfiguration(builder.Configuration);

        // Adicionar serviços ao container
        builder.Services.AddScoped<IContatoService, ContatoService>();
        
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        
        // Configuração do middleware
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DeletarContato API V1"));
        }

        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}