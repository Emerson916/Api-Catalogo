using System.Text.Json.Serialization;
using APICatalogo.Context;
using APICatalogo.Extensions;
using APICatalogo.Filters;
using APICatalogo.Logging;
using APICatalogo.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços aos Controllers
builder.Services.AddControllers(options => { options.Filters.Add(typeof(ApiExcepitionFilter)); }).AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//Adicionando Filtro personalizado
builder.Services.AddScoped<ApiLoggingFilter>();
builder.Services.AddScoped<IcategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

//Loggers personalizados
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    logLevel = LogLevel.Information
}));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Conexão com o Postgres
var PsqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(PsqlConnection));

//Pegando informações do arquivo de configurações
var valor1 = builder.Configuration["chave1"];
var chave2 = builder.Configuration["secao1:chave2"];

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();
// app.UseAuthentication();
// app.UseAuthorization();
app.MapControllers();

app.Run();