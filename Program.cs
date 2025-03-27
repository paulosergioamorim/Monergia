using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Monergia.Configs;
using Monergia.DbContexts;
using Monergia.Models;
using Monergia.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Monergia WEB API",
        Description = ".NET 8 Web API"
    });
    // To Enable authorization using Swagger (JWT)
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    var requirement = new OpenApiSecurityRequirement
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
            []
        }
    };
    swagger.AddSecurityRequirement(requirement);
});

builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("POSTGRES_CONNECTION_STRING")));

builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddSingleton<IPasswordHasher<Acesso>, PasswordHasher<Acesso>>();
builder.Services.AddSingleton<IFilialService, FilialService>();
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddSingleton<IRotaService, RotaService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(jwtOptions =>
    {
        jwtOptions.RequireHttpsMetadata = builder.Environment.IsProduction();
        jwtOptions.Authority = builder.Configuration["Jwt:Issuer"];
        jwtOptions.Audience = builder.Configuration["Jwt:Audience"];
        jwtOptions.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(Policies.ProprietarioPolicy,
        p => p.RequireClaim(Policies.UserRoleClaimName, Policies.ProprietarioRoleValue))
    .AddPolicy(Policies.AdministradorPolicy,
        p => p.RequireRole(Policies.UserRoleClaimName, Policies.ProprietarioRoleValue, Policies.AdministradorRoleValue))
    .AddPolicy(Policies.SupervisorPolicy,
        p => p.RequireRole(Policies.UserRoleClaimName, Policies.ProprietarioRoleValue, Policies.AdministradorRoleValue,
            Policies.SupervisorRoleValue))
    .AddPolicy(Policies.GestorPolicy,
        p => p.RequireRole(Policies.UserRoleClaimName, Policies.ProprietarioRoleValue, Policies.AdministradorRoleValue,
            Policies.GestorRoleValue))
    .AddPolicy(Policies.TecnicoPolicy,
        p => p.RequireRole(Policies.UserRoleClaimName, Policies.ProprietarioRoleValue, Policies.AdministradorRoleValue,
            Policies.SupervisorRoleValue, Policies.TecnicoRoleValue));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await using (var db = app.Services.GetService<IDbContextFactory<AppDbContext>>()!.CreateDbContext())
    await db.Database.MigrateAsync();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();