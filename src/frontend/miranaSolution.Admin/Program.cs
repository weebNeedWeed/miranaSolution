using Microsoft.AspNetCore.Authentication.Cookies;
using miranaSolution.Admin.Services.Interfaces;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.SchemaName = "dbo";  
    options.TableName = "SQLSessions";  
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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

var configureClient = (HttpClient client) =>
{
    client.BaseAddress = new Uri(builder.Configuration["BaseAddress"]);
};

builder.Services.AddRefitClient<IUsersApiService>().ConfigureHttpClient(configureClient);
builder.Services.AddRefitClient<IBooksApiService>().ConfigureHttpClient(configureClient);
builder.Services.AddRefitClient<IAuthorsApiService>().ConfigureHttpClient(configureClient);

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

app.Run();