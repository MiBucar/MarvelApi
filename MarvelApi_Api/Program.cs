using MarvelApi_Api;
using MarvelApi_Api.ActionFilters.Hero;
using MarvelApi_Api.Data;
using MarvelApi_Api.Repository;
using MarvelApi_Api.Repository.Implementation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MarvelApi")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IHeroRepository, HeroRepository>();
builder.Services.AddScoped<ValidateHeroExistsAttribute>();
builder.Services.AddScoped<ValidateHeroPropertiesAttribute>();
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