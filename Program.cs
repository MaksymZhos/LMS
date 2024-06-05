using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppDbContext>();

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Seed the database with initial data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
        SeedUsers(userManager).Wait();
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapRazorPages();

app.Run();

static async Task SeedUsers(UserManager<IdentityUser> userManager)
{
    if (userManager.Users.Any())
    {
        return;   // DB has been seeded
    }

    var user = new IdentityUser
    {
        UserName = "testuser",
        NormalizedUserName = "TESTUSER",
        Email = "testuser@example.com",
        NormalizedEmail = "TESTUSER@EXAMPLE.COM",
        EmailConfirmed = true,
    };

    var passwordHasher = new PasswordHasher<IdentityUser>();
    user.PasswordHash = passwordHasher.HashPassword(user, "Test@1234");

    var result = await userManager.CreateAsync(user);

    if (!result.Succeeded)
    {
        throw new Exception("Failed to create test user.");
    }
}
