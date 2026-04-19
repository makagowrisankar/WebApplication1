using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ Use SQLite database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

var app = builder.Build();

// ✅ Bind to Render port
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Urls.Add($"http://0.0.0.0:{port}");

app.UseDefaultFiles();
app.UseStaticFiles();

// ✅ Create DB + default user (ONLY ONCE)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    db.Database.EnsureCreated();

    if (!db.Users.Any())
    {
        db.Users.Add(new User
        {
            Email = "admin@gmail.com",
            Password = "1234"
        });

        db.SaveChanges();
    }
}

// 🔐 REGISTER
app.MapPost("/register", async (AppDbContext db, User user) =>
{
    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Ok("User Registered");
});

// 🔐 LOGIN
app.MapPost("/login", async (AppDbContext db, User loginUser) =>
{
    var user = await db.Users
        .FirstOrDefaultAsync(u => u.Email == loginUser.Email && u.Password == loginUser.Password);

    if (user == null)
        return Results.Unauthorized();

    return Results.Ok("Login Success");
});

// 📍 GET PROVIDERS
app.MapGet("/providers", async (AppDbContext db) =>
{
    return await db.Providers.ToListAsync();
});

// 📅 BOOK SERVICE
app.MapPost("/book", async (AppDbContext db, Booking booking) =>
{
    db.Bookings.Add(booking);
    await db.SaveChangesAsync();
    return Results.Ok("Booked Successfully");
});

app.Run();