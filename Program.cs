using FiapStore.Entities;
using FiapStore.Interfaces;
using FiapStore.Logging;
using FiapStore.Repositories;
using FiapStore.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuração do log custom
builder.Logging.ClearProviders();
builder.Logging.AddProvider(new CustomLoggerProvider(new()
{
    LogLevel = LogLevel.Information
}));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUsuarioRepository, EFUsuarioRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FiapStore", Version = "v1.2" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
            "JWT Authorization Header - utilizado com Bearer Authentication\r\n" +
            "Digite 'Bearer' [espaço] e então seu token no campo abaixo.\r\n" +
            "Exemplo (informar sem as aspas): 'Bearer 1234abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    var xmlFile = $"{Assembly
        .GetExecutingAssembly()
        .GetName()
        .Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    c.IncludeXmlComments(xmlPath);
});

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("Secret"));

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

using var serviceScoped = app.Services.GetRequiredService<IServiceScopeFactory>()
                .CreateScope();

using var context = serviceScoped.ServiceProvider
    .GetService<ApplicationDbContext>();

if (!context.Set<Usuario>().Any())
{
    var usuario = new Usuario()
    {
        Nome = "Administrador",
        NomeUsuario = "admin",
        Senha = "admin",
        Permissao = FiapStore.Enum.TipoPermissao.Administrador
    };

    context.Set<Usuario>().Add(usuario);
    context.SaveChanges();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseReDoc(c =>
{
    c.DocumentTitle = "REDOC FiapStore API";
    c.RoutePrefix = "";
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
