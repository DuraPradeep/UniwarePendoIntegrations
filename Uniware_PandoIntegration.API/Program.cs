
using Uniware_PandoIntegration.APIs;
using Uniware_PandoIntegration.DataAccessLayer;
using Serilog;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Data.Common;
using Uniware_PandoIntegration.API;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.Data;
using Uniware_PandoIntegration.API.Folder;

var builder = WebApplication.CreateBuilder(args);


IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
builder.Services.Add(new ServiceDescriptor(typeof(UniwareDB), new UniwareDB(configuration.GetConnectionString("DBConnection"), configuration)));
builder.Services.Add(new ServiceDescriptor(typeof(SPWrapper), new SPWrapper(configuration.GetConnectionString("DBConnection"), configuration)));
builder.Services.Add(new ServiceDescriptor(typeof(BasicAuthenticationFilterAttribute), new BasicAuthenticationFilterAttribute(configuration)));
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var Key = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = configuration["JWT:Issuer"],
    ValidAudience = configuration["JWT:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Key),
    RequireExpirationTime = false,
};
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{

    o.SaveToken = true;
    o.TokenValidationParameters = tokenValidationParameters;
    o.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
            }
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddControllers();
builder.Services.AddSession();
builder.Services.AddMvc()
       .AddSessionStateTempDataProvider();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton(new BasicAuthenticationFilterAttribute());
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IUniwarePando>(new GenerateToken(configuration));
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                    }
                },
                new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    context.Response.GetTypedHeaders().CacheControl =
      new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
      {
          Public = true,
          MaxAge = TimeSpan.FromSeconds(10)
      };
    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
      new string[] { "Accept-Encoding" };

    await next();
});
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
