using Application;
using Infrastructure;
using Persistence;
using Persistence.Options;
using Presentation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .Scan(
        selector => selector
            .FromAssemblies(
                Infrastructure.AssemblyReference.Assembly,
                Persistence.AssemblyReference.Assembly)
            .AddClasses(c => c
                .Where(t => t != typeof(ConfigureDatabaseOptions)),
                publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

builder.Services
    .AddApplication()
    .AddPersistence()
    .AddInfrastructure()
    .AddPresentation();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UsePersistence();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();