using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ DB connection (change if needed)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();


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