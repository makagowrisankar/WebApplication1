using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// DB connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// static files
app.UseDefaultFiles();
app.UseStaticFiles();


// 🔐 LOGIN API
app.MapPost("/login", async (AppDbContext db, User user) =>
{
    var foundUser = await db.Users
        .FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);

    return Results.Json(new { success = foundUser != null });
});


// 🔍 GET PROVIDERS (FILTER)
app.MapGet("/providers", async (AppDbContext db, string service, double lat, double lng) =>
{
    var providers = await db.Providers
        .Where(p => p.Service.ToLower() == service.ToLower())
        .ToListAsync();

    var result = providers.Select(p => new
    {
        p.Name,
        p.Service,
        Distance = Math.Sqrt(
            Math.Pow(p.Latitude - lat, 2) +
            Math.Pow(p.Longitude - lng, 2)
        )
    });

    return Results.Json(result);
});


// 📦 BOOK SERVICE
app.MapPost("/book", async (AppDbContext db, Booking booking) =>
{
    db.Bookings.Add(booking);
    await db.SaveChangesAsync();

    return Results.Json(new { success = true });
});


// 📋 GET BOOKINGS (🔥 NEW API)
app.MapGet("/bookings", async (AppDbContext db, string email) =>
{
    var data = await db.Bookings
        .Where(b => b.UserEmail == email)
        .ToListAsync();

    return Results.Json(data);
});

app.MapDelete("/bookings/{id}", async (AppDbContext db, int id) =>
{
    var booking = await db.Bookings.FindAsync(id);

    if (booking == null)
        return Results.Json(new { success = false });

    db.Bookings.Remove(booking);
    await db.SaveChangesAsync();

    return Results.Json(new { success = true });
});


app.Run();