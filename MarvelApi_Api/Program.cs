using System.Runtime.InteropServices;
using System.Text;
using MarvelApi_Api;
using MarvelApi_Api.ActionFilters.Character;
using MarvelApi_Api.ActionFilters.Teams;
using MarvelApi_Api.Data;
using MarvelApi_Api.ExceptionFilters;
using MarvelApi_Api.Helpers;
using MarvelApi_Api.Repository;
using MarvelApi_Api.Repository.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);

if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
{
    builder.Configuration.AddJsonFile("appsettings.Mac.json", optional: true, reloadOnChange: true);
}
else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    builder.Configuration.AddJsonFile("appsettings.PC.json", optional: true, reloadOnChange: true);
}

var customLogTheme = new AnsiConsoleTheme(new Dictionary<ConsoleThemeStyle, string>
{
    [ConsoleThemeStyle.Text] = "\x1b[37m",
    [ConsoleThemeStyle.SecondaryText] = "\x1b[37m",
    [ConsoleThemeStyle.TertiaryText] = "\x1b[30m",
    [ConsoleThemeStyle.Invalid] = "\x1b[31m",
    [ConsoleThemeStyle.Null] = "\x1b[31m",
    [ConsoleThemeStyle.Name] = "\x1b[37m",
    [ConsoleThemeStyle.String] = "\x1b[32m",
    [ConsoleThemeStyle.Number] = "\x1b[33m",
    [ConsoleThemeStyle.Boolean] = "\x1b[33m",
    [ConsoleThemeStyle.Scalar] = "\x1b[33m",
    [ConsoleThemeStyle.LevelVerbose] = "\x1b[30m",
    [ConsoleThemeStyle.LevelDebug] = "\x1b[30m",
    [ConsoleThemeStyle.LevelInformation] = "\x1b[34m",
    [ConsoleThemeStyle.LevelWarning] = "\x1b[33m",
    [ConsoleThemeStyle.LevelError] = "\x1b[31m",
    [ConsoleThemeStyle.LevelFatal] = "\x1b[35m"
});

Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
    .WriteTo.Console(theme: customLogTheme)
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MarvelApi")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("ApiSettings:Secret").Value))
    };
});
builder.Services.AddAuthorization();

builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddTransient<ApiFilterResponseHelper>();
builder.Services.AddTransient<ApiResponseHelper>();
#region Register API Filters
builder.Services.AddScoped<ValidateCharacterExistsAttribute>();
builder.Services.AddScoped<ValidateCharacterCreateAndUpdateAttribute>();
builder.Services.AddScoped<ValidateCharactersNotRelatedAttribute>();
builder.Services.AddScoped<ValidateTeamExistsAttribute>();
builder.Services.AddScoped<ValidateTeamCreateAndUpdate>();
#endregion

builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();