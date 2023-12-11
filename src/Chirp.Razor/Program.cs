using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

//Test if Tests are misbehaving?

var builder = WebApplication.CreateBuilder(args);

// Get connection string from user secrets
var connection = String.Empty;
if (builder.Environment.IsDevelopment())
{
    connection = builder.Configuration["AZURE_SQL_CONNECTIONSTRING"];
}
else
{
    connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
}

// Add services to the container.

builder.Services.AddDbContext<ChirpDBContext>(options => options.UseSqlServer(connection));
builder.Services.AddScoped<ICheepRepository, CheepRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddMemoryCache();

// Authentication with AD B2C
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureADB2C"));
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

Trace.WriteLine("Programmet kører");

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ChirpDBContext>();

    context.Database.Migrate();

    //Then you can use the context to seed the database for example
    DbInitializer.SeedDatabase(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCookiePolicy(new CookiePolicyOptions()
{
    Secure = CookieSecurePolicy.Always
});

app.UseRouting();

app.MapControllers();
app.MapRazorPages();


Trace.WriteLine("Programmet skal til at runne");

app.Run();

Trace.WriteLine("programmet er kørt");

public partial class Program { }
