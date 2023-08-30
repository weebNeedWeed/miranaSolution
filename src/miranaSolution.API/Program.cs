using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using miranaSolution.API.Extensions;
using miranaSolution.API.Filters;
using miranaSolution.API.HealthChecks;
using miranaSolution.Data;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(x =>
{
    // x.Filters.Add<ModelStateFilter>();
    x.Filters.Add<ApiExceptionFilter>();
});
builder.Services.Configure<ApiBehaviorOptions>(x => { x.SuppressModelStateInvalidFilter = true; });

// Add project's logical layers
builder.Services
    .AddDataLayer()
    .AddBusinessLayer()
    .Configure<SeedingOptions>(builder.Configuration.GetSection(SeedingOptions.SectionName));

builder.Services
    .AddAuth()
    .AddSwagger();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Default",
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("Database");

var app = builder.Build();

// Seeding data
using (var serviceScope = app.Services.CreateScope())
{
    var serviceProvider = serviceScope.ServiceProvider;
    var dbContext = serviceProvider.GetRequiredService<MiranaDbContext>();
    var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
    var seedingOptions = serviceProvider.GetRequiredService<IOptions<SeedingOptions>>();

    var seeder = new SampleDataSeeder(dbContext,userManager,roleManager,seedingOptions);
    await seeder.SeedAllAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseCors("Default");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapHealthChecks("/_health");

app.MapControllers();

app.Run();