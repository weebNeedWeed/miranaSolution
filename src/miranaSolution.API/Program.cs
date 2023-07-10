using Microsoft.AspNetCore.Mvc;
using miranaSolution.API.Filters;
using miranaSolution.API.Extensions;
using miranaSolution.API.HealthChecks;
using miranaSolution.Services;
using miranaSolution.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(x => x.Filters.Add<HandleModelStateFilter>());
builder.Services.Configure<ApiBehaviorOptions>(x => { x.SuppressModelStateInvalidFilter = true; });

// Add project's logical layers
builder.Services
    .AddDataLayer()
    .AddAuth(builder.Configuration)
    .AddSwagger();

builder.Services.AddBusinessLayer();

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