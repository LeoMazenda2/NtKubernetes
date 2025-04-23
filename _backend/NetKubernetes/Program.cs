using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetKubernetes.Data;
using NetKubernetes.Data.Imoveis;
using NetKubernetes.Data.Usuarios;
using NetKubernetes.Middleware;
using NetKubernetes.Models;
using NetKubernetes.Profiles;
using NetKubernetes.Token;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.LogTo(Console.WriteLine, new[] {
        DbLoggerCategory.Database.Command.Name},
        LogLevel.Information).EnableSensitiveDataLogging();

    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")!);
});

builder.Services.AddScoped<IImovelRepository, ImovelRepository>();

builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mapperConfig = new AutoMapper.MapperConfiguration(mc =>
{
    mc.AddProfile(new ImovelProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var builderSecurity = builder.Services.AddIdentityCore<Usuario>();
var identityBuilder = new IdentityBuilder(builderSecurity.UserType, builder.Services);
identityBuilder.AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
identityBuilder.AddSignInManager<SignInManager<Usuario>>();
builder.Services.AddSingleton<ISystemClock, SystemClock>();
builder.Services.AddScoped<IJwtGeradorToken, JwtGeradorToken>();
builder.Services.AddScoped<IUsuarioSessao, UsuarioSessao>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Minha palavra Secreta"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateAudience = false,
            ValidateIssuer = false,
        };
    });

builder.Services.AddCors(o => o.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*")
        .AllowAnyMethod()
        .AllowAnyHeader();
}));


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ManagerMiddleware>();

app.UseAuthentication();
app.UseCors("corsapp");
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


using (var ambiente = app.Services.CreateAsyncScope())
{
    var services = ambiente.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<Usuario>>();
        var context = services.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();
        await LoadDatabase.InserirDados(context, userManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro ao na migração dos dados");
    }
}


app.Run();
