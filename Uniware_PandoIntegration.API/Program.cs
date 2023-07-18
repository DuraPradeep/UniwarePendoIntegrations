
using Uniware_PandoIntegration.APIs;
using Uniware_PandoIntegration.DataAccessLayer;
using Serilog;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Data.Common;
using Uniware_PandoIntegration.API;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);


IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
builder.Services.Add(new ServiceDescriptor(typeof(UniwareDB), new UniwareDB(configuration.GetConnectionString("DBConnection"), configuration)));
builder.Services.Add(new ServiceDescriptor(typeof(SPWrapper), new SPWrapper(configuration.GetConnectionString("DBConnection"), configuration)));
builder.Services.Add(new ServiceDescriptor(typeof(BasicAuthenticationFilterAttribute), new BasicAuthenticationFilterAttribute(configuration)));
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSession();
builder.Services.AddMvc()
       .AddSessionStateTempDataProvider();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton(new BasicAuthenticationFilterAttribute());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseSession();
app.MapControllers();

app.Run();
