using Microsoft.AspNetCore.Authentication.Cookies;
using miranaSolution.Admin.Services;
using miranaSolution.Admin.Services.Interfaces;
using Refit;

var builder = WebApplication.CreateBuilder(args);

void ConfigureClient(HttpClient client)
{
    client.BaseAddress = new Uri(builder!.Configuration["BaseAddress"]);
}

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Forbidden/";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;

        options.LoginPath = "/Auth/Login";
    });

builder.Services.AddRefitClient<IUsersApiService>().ConfigureHttpClient(ConfigureClient);
builder.Services.AddRefitClient<IBooksApiService>().ConfigureHttpClient(ConfigureClient);

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();