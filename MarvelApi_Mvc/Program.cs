using MarvelApi_Mvc.Handlers;
using MarvelApi_Mvc.Services.Implementation;
using MarvelApi_Mvc.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ICharacterService, CharacterService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ISelectListItemGetters, SelectListItemGetters>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBaseService, BaseService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<AuthorizationHandler>();
builder.Services.AddHttpClient("MarvelApi").AddHttpMessageHandler<AuthorizationHandler>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();