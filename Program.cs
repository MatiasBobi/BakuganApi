using BakuganApi.Data;
using BakuganAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using BakuganApi.Utils;
using BakuganApi.services;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);

// Configuracion cadena de conexion

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro de servicios
builder.Services.AddScoped<IBakuganService, BakuganService>();
builder.Services.AddScoped<IUsuarioService, UsuariosService>();
//

// Singleton de utilidades
builder.Services.AddSingleton<Utilidades>();

// Configuracion de autenticacion JWT
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:key"]!))
    };
    config.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new
            {
                error = "No autorizado",
                mensaje = "Tenes que iniciar sesión para poder consultar."
            });
            return context.Response.WriteAsync(result);
        },

        OnForbidden = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new
            {
                error = "Acceso denegado.",
                mensaje = "No tenes los permisos suficientes para utilizar este endpoint."
            });
            return context.Response.WriteAsync(result);
        }
    };
});

builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.MapGet("/", () => "Bienvenido a mi bakugan API");

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
