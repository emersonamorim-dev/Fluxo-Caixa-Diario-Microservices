using FluentValidation;
using FluxoCaixaDiarioMicroservice.Application.Services;
using FluxoCaixaDiarioMicroservice.Application.UseCases;
using FluxoCaixaDiarioMicroservice.Domain.Interfaces;
using FluxoCaixaDiarioMicroservice.Domain.Repositories;
using FluxoCaixaDiarioMicroservice.Infrastructure.Config;
using FluxoCaixaDiarioMicroservice.Infrastructure.Logging;
using FluxoCaixaDiarioMicroservice.Infrastructure.Redis;
using FluxoCaixaDiarioMicroservice.Presentation.Middleware;
using MongoDB.Driver;
using RabbitMQ.Client;
using Serilog;
using StackExchange.Redis;
using System.Reflection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;


var builder = WebApplication.CreateBuilder(args);

// Configura Serilog para Logging
LoggingConfig.ConfigureLogging(builder.Configuration);
builder.Host.UseSerilog();

// Adiciona serviços ao contêiner
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.WriteIndented = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Adiciona Redis - Utilizando RedisServiceExtensions
builder.Services.AddRedis(builder.Configuration);

// Registra IConnectionMultiplexer para Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(configuration);
});

// Registra serviços para LancamentosController e ConsolidadoDiarioController
builder.Services.AddScoped<ILancamentoService, LancamentoService>();
builder.Services.AddScoped<IConsolidadoDiarioService, ConsolidadoDiarioService>();
builder.Services.AddScoped<ILancamentoRepository, LancamentoRepository>();
builder.Services.AddScoped<IConsolidadoDiarioRepository, ConsolidadoDiarioRepository>();
builder.Services.AddScoped<IRedisCacheService, RedisCacheService>(); 

// Registra FluentValidation
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Registra MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

// Configura MongoDB
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var mongoClient = new MongoClient(builder.Configuration.GetConnectionString("MongoDb"));
    return mongoClient.GetDatabase(builder.Configuration["MongoSettings:DatabaseName"]);
});

// Adiciona Redis ConnectionMultiplexer
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetSection("Redis:ConnectionString").Value;
    if (string.IsNullOrEmpty(configuration))
    {
        throw new ArgumentNullException(nameof(configuration), "A configuração do Redis não pode ser nula. Verifique se a chave 'Redis:ConnectionString' está presente no arquivo de configuração.");
    }
    return ConnectionMultiplexer.Connect(configuration);
});

// Configura RabbitMQ com tratamento de erros
builder.Services.AddSingleton<IConnection>(sp =>
{
    try
    {
        var factory = new ConnectionFactory
        {
            HostName = builder.Configuration["RabbitMQ:HostName"] ?? throw new ArgumentNullException("HostName"),
            UserName = builder.Configuration["RabbitMQ:UserName"] ?? "guest",
            Password = builder.Configuration["RabbitMQ:Password"] ?? "guest",
            Port = int.Parse(builder.Configuration["RabbitMQ:Port"] ?? "5672")
        };

        return factory.CreateConnection();
    }
    catch (Exception ex)
    {
        throw new Exception("Failed to create RabbitMQ connection", ex);
    }
});

// Registra os UseCases
builder.Services.AddScoped<IAdicionarLancamentoUseCase, AdicionarLancamentoUseCase>();
builder.Services.AddScoped<IConsolidarSaldoDiarioUseCase, ConsolidarSaldoDiarioUseCase>();

// Configura a representação do Guid
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

var app = builder.Build();

// Adiciona Elastic APM se estiver configurado
// Se o Elastic APM for utilizado, adicione o pacote correto e use o middleware
// app.UseElasticApm(builder.Configuration);

// Configura o pipeline de requisição HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// escutando na porta
app.Urls.Add("http://*:80");

app.Run();

