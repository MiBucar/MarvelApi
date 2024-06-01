using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using MarvelApi_Api;
using MarvelApi_Api.ActionFilters.Character;
using MarvelApi_Api.ActionFilters.Teams;
using MarvelApi_Api.Data;
using MarvelApi_Api.ExceptionFilters;
using MarvelApi_Api.Helpers;
using MarvelApi_Api.Repository;
using MarvelApi_Api.Repository.Implementation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
{
    builder.Configuration.AddJsonFile("appsettings.Mac.json", optional: true, reloadOnChange: true);
}
else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    builder.Configuration.AddJsonFile("appsettings.PC.json", optional: true, reloadOnChange: true);
}

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MarvelApi")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});
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

app.UseHttpsRedirection();
app.MapControllers();

app.Run();