using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MvcLoginTemplate;
using MvcLoginTemplate.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configure the application cookie to redirect to Account/Index when unauthenticated
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Index";  // Redirect to this path for unauthenticated users
    options.AccessDeniedPath = "/Home/Privacy";  // Redirect here if access is denied
});


/*builder.Services.AddIdentity<ApplicationUser,IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();*/


builder.Services.AddControllersWithViews();


var app = builder.Build();

// seeding


// Seed default user and roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    // Create roles if they don't exist
    string[] roleNames = { "Admin", "User" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Create default user "sneh"
    var defaultUser = new ApplicationUser { FirstName = "Sneh", LastName = "Gour", UserName = "Sneh", Email = "sneh@example.com", EmailConfirmed = true };
    string defaultPassword = "Sneh@123";

    if (await userManager.FindByNameAsync(defaultUser.UserName) == null)
    {
        var createPowerUser = await userManager.CreateAsync(defaultUser, defaultPassword);
        if (createPowerUser.Succeeded)
        {
            await userManager.AddToRoleAsync(defaultUser, "Admin");
        }
    }
}



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
