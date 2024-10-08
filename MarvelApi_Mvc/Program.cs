using MarvelApi_Mvc;
using MarvelApi_Mvc.Handlers;
using MarvelApi_Mvc.Services.Implementation;
using MarvelApi_Mvc.Services.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ICharacterService, CharacterService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ISelectListItemGetters, SelectListItemGetters>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IMailReceiverService, MailReceiverService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<AuthorizationHandler>();
builder.Services.AddHttpClient("MarvelApi").AddHttpMessageHandler<AuthorizationHandler>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(options =>
              {
                  options.Cookie.HttpOnly = true;
                  options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                  options.LoginPath = "/User/Login";
                  options.AccessDeniedPath = "/User/AccessDenied";
                  options.SlidingExpiration = true;
              });

builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
			name: "characters",
			pattern: "Characters",
			defaults: new { controller = "Character", action = "IndexCharacters" });

app.MapControllerRoute(
			name: "character",
			pattern: "Characters/Character",
			defaults: new { controller = "Character", action = "IndexCharacter" });

app.MapControllerRoute(
			name: "teams",
			pattern: "Teams",
			defaults: new { controller = "Team", action = "IndexTeams" });

app.MapControllerRoute(
			name: "team",
			pattern: "Teams/Team",
			defaults: new { controller = "Team", action = "IndexTeam" });

app.MapControllerRoute(
			name: "adminCharacters",
			pattern: "AdminDashboard/Characters",
			defaults: new { controller = "AdminDashboard", action = "IndexDashboardCharacters" });

app.MapControllerRoute(
			name: "adminTeams",
			pattern: "AdminDashboard/Teams",
			defaults: new { controller = "AdminDashboard", action = "IndexDashboardTeams" });

app.MapControllerRoute(
            name: "adminDashboard",
            pattern: "AdminDashboard",
            defaults: new { controller = "AdminDashboard", action = "IndexAdminDashboard" });

app.MapControllerRoute(
            name: "accessDenied",
            pattern: "AccessDenied",
            defaults: new { controller = "User", action = "AccessDenied" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();