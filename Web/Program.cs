using Business;
using Business.Interfaces;
using Business.Services;
using Data.Interfaces;
using Data.Repositories;
using Entity.context;
using Entity.Model;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Utilities.Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Utilities;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var dbProvider = configuration["DatabaseProvider"];

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(config => {
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(config => {
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey= true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]!))
    };
});

builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<PersonService>();

builder.Services.AddScoped<IFormRepository, FormRepository>();
builder.Services.AddScoped<FormService>();

builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
builder.Services.AddScoped<ModuleService>();

builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<PermissionService>();

builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<RolService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserServices,UserService>();


builder.Services.AddScoped<IFormModuleRepository, FormModuleRepository>();
builder.Services.AddScoped<FormModuleService>();

builder.Services.AddScoped<IRolUserRepository, RolUserRepository>();
builder.Services.AddScoped<RolUserService>();

builder.Services.AddScoped<IRolFormPermissionRepository, RolFormPermissionRepository>();
builder.Services.AddScoped<RolFormPermissionService>();

builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<Jwt>();


builder.Services.AddMapster(); // agrega IMapper
MapsterConfig.RegisterMappings(); // registra los mapeos

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    switch (dbProvider)
    {
        case "SqlServer":
            options.UseSqlServer(configuration.GetConnectionString("DefaultConn"));
            break;
        default:
            throw new Exception("Proveedor de base de datos no soportado");
    }
});

var origenesPermitidos = builder.Configuration
    .GetValue<string>("OrigenesPermitidos")!
    .Split(",");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins(origenesPermitidos)
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

//builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
//opciones.UseSqlServer("name=DefaultConnection"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
