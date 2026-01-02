using Microsoft.EntityFrameworkCore;
using MassTransit;
using OfferService.Infrastructure.Data;
using OfferService.Infrastructure.Repositories;
using OfferService.Infrastructure.Messaging;
using OfferService.Infrastructure.ExternalServices;
using OfferService.Application.Interfaces;
using OfferService.Application.Services;
using OfferService.Application.Mapping;
using OfferService.Domain.Interfaces;
using OfferService.Api.Middleware;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() 
    { 
        Title = "Offer Service API", 
        Version = "v1",
        Description = "A comprehensive microservice for managing vehicle offers with clean architecture, PostgreSQL, and RabbitMQ messaging."
    });
    
    // Include XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Database Configuration
builder.Services.AddDbContext<OfferDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper Configuration
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Repository Pattern
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ISellerRepository, SellerRepository>();
builder.Services.AddScoped<IOfferRepository, OfferRepository>();

// Application Services
builder.Services.AddScoped<ISellerService, SellerService>();
builder.Services.AddScoped<IOfferService, OfferService.Application.Services.OfferService>();

// External API Services
builder.Services.AddHttpClient<IPurchaseApiService, PurchaseApiService>();
builder.Services.AddHttpClient<ITransportApiService, TransportApiService>();

// MassTransit Configuration
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMQ") ?? "rabbitmq://localhost");
        cfg.ConfigureEndpoints(context);
    });
});

// Event Publisher
builder.Services.AddScoped<IEventPublisher, MassTransitEventPublisher>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

// Add custom middleware
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();

// Enable Swagger in all environments for containerized deployment
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Offer Service API v1");
    c.RoutePrefix = "swagger"; // Set Swagger UI at /swagger
});

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// Database Migration and Seeding
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OfferDbContext>();
    
    // Ensure database is created and migrated
    context.Database.EnsureCreated();
    
    // In production, use: context.Database.Migrate();
}

app.Run();
