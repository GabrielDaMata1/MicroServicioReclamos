using Application.Command;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Consumer;
using Infrastructure.Persistance;
using Infrastructure.Repositories.MongoDB;
using Infrastructure.Repositories.PostgreSQL;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Reflection;
using Application.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Mi API",
        Version = "v1",
        Description = "Documentaci�n de mi API usando Swagger"
    });
});


builder.Services.AddDbContext<SubastaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"),
        b => b.MigrationsAssembly("Infrastructure")));


var mongoClient = new MongoClient("mongodb://localhost:27017");
builder.Services.AddSingleton<IMongoClient>(mongoClient);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddHttpClient<UsuarioService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5001/api/usuarios/");
});

builder.Services.AddHttpClient<SubastaService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5003/api/Subastas/");
});

builder.Services.AddHttpClient<NotificacionService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5287/api/Notification/");
});


builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RegistrarReclamoCommand).Assembly));

builder.Services.AddSignalR();

builder.Services.AddScoped<IReclamoMongoRepository, ReclamoMongoRepository>();
builder.Services.AddScoped<IResolucionReclamoMongoRepository, ResolucionReclamoMongoRepository>();
builder.Services.AddScoped<IResolucionReclamoPostgreSQLRepository, ResolucionReclamoPostgreSQLRepository>();
builder.Services.AddScoped<IReclamoPostgreSQLRepository, ReclamoPostgreSQLRepository>();
builder.Services.AddScoped<IReclamoPremioMongoRepository, ReclamoPremioMongoRepository>();
builder.Services.AddScoped<IReclamoPremioPostgreSQLRepository, ReclamoPremioPostgreSQLRepository>();
builder.Services.AddScoped<IReclamoService, ReclamoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ISubastaService, SubastaService>();
builder.Services.AddScoped<INotificacionService, NotificacionService>();



// Configuraci�n de RabbitMQ con MassTransit
builder.Services.AddMassTransit(x =>
{

    x.AddConsumer<ReclamoRegistradoConsumer>();
    x.AddConsumer<ResolucionReclamoRegistradaConsumer>();
    x.AddConsumer<ReclamoPremioRegistradoConsumer>();


    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("reclamo-registrado-queue", e =>
        {
            e.ConfigureConsumer<ReclamoRegistradoConsumer>(context);
        });


        cfg.ReceiveEndpoint("resolucionReclamo-registrado-queue", e =>
        {
            e.ConfigureConsumer<ResolucionReclamoRegistradaConsumer>(context);
        });

        cfg.ReceiveEndpoint("reclamoPremio-registrado-queue", e =>
        {
            e.ConfigureConsumer<ReclamoPremioRegistradoConsumer>(context);
        });

    });
});


var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API v1");
    });
}
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

