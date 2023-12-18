using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

// Initializes a web application builder
var builder = WebApplication.CreateBuilder(args);

// Determines the database connection string based on the environment.

var connection = String.Empty;

if (builder.Environment.IsDevelopment())
{
    connection = builder.Configuration["AZURE_SQL_CONNECTIONSTRING"];
}
else
{
    connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
}

// Configures the ChirpDBContext with selected database connection.
builder.Services.AddDbContext<ChirpDBContext>(options => options.UseSqlServer(connection));

// Adds repositories and memory cache
builder.Services.AddScoped<ICheepRepository, CheepRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddMemoryCache();

// Authentication with AD B2C
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureADB2C"));
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

// Builds the application
var app = builder.Build();

// Database migration and initialization
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ChirpDBContext>();

    // Applies database migrations
    context.Database.Migrate();

    // Seed the database with initial data
    DbInitializer.SeedDatabase(context);
}

// Configures the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Configure cookie policy with secure settings.
app.UseCookiePolicy(new CookiePolicyOptions()
{
    Secure = CookieSecurePolicy.Always
});

//Configure routing for controllers and Razor Pages
app.UseRouting();

app.MapControllers();
app.MapRazorPages();

// Runs the application
app.Run();


public partial class Program { }
