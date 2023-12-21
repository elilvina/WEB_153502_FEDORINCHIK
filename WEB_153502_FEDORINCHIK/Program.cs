using Microsoft.AspNetCore.Authentication;
using NuGet.Packaging;
using Serilog;
using WEB_153502_FEDORINCHIK.Domain.Models;
using WEB_153502_FEDORINCHIK.Middleware;
using WEB_153502_FEDORINCHIK.Models;
using WEB_153502_FEDORINCHIK.Services.CartService;
using WEB_153502_FEDORINCHIK.Services.GameGenreService;
using WEB_153502_FEDORINCHIK.Services.GameService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

//builder.Services.AddScoped<IGameGenreService, MemoryGameGenreService>();
//builder.Services.AddScoped<IGameService, MemoryGameService>();

UriData uriData = builder.Configuration.GetSection("UriData").Get<UriData>()!;

builder.Services.AddScoped<Cart, SessionCart>();

builder.Services.AddHttpClient<IGameService, ApiGameService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));
builder.Services.AddHttpClient<IGameGenreService, ApiGameGenreService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = "cookie";
    opt.DefaultChallengeScheme = "oidc";
})
    .AddCookie("cookie")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
        options.ClientId = builder.Configuration["InteractiveServiceSettings:ClientId"];
        options.ClientSecret = builder.Configuration["InteractiveServiceSettings:ClientSecret"];
        // Получить Claims пользователя
        options.GetClaimsFromUserInfoEndpoint = true;
        options.ResponseType = "code";
        options.ResponseMode = "query";
        options.SaveTokens = true;
    });

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

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

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapRazorPages().RequireAuthorization();

app.UseMiddleware<LoggingMiddleware>(logger);

app.Run();
