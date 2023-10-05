using BackendFirma.Context;
using BackendFirma.Interfaces;
using BackendFirma.Providers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(options => options.AddPolicy("AllowWebapp",
                                        builder => builder.AllowAnyOrigin()
                                                        .AllowAnyHeader()
                                                        .AllowAnyMethod()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<FirmaDBContext>(opt =>
                                             opt.UseNpgsql(builder.Configuration.GetConnectionString("MyDB")));
builder.Services.AddScoped<IFirmaProvider,FirmaProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowWebapp");

app.UseAuthorization();

app.MapControllers();

app.Run();
