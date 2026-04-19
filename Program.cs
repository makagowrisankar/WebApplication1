using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// ✅ CORS (important)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors();

// ✅ static files
app.UseDefaultFiles();
app.UseStaticFiles();


// 🔐 LOGIN (dummy)
app.MapPost("/login", (User user) =>
{
    if (user.Email == "admin@gmail.com" && user.Password == "1234")
        return Results.Json(new { success = true });

    return Results.Json(new { success = false });
});


// 🔍 FIND SERVICES (dummy data)
app.MapGet("/providers", (string service) =>
{
    var data = new[]
    {
        new { Name = "Ravi Plumber", Service = "plumber" },
        new { Name = "Kumar Electrician", Service = "electrician" },
        new { Name = "Suresh Cleaner", Service = "cleaner" }
    };

    return Results.Json(data);
});


// 📦 BOOK SERVICE (dummy)
app.MapPost("/book", () =>
{
    return Results.Json(new { success = true });
});


// ✅ PORT FIX (VERY IMPORTANT)
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Urls.Add($"http://0.0.0.0:{port}");

app.Run();


// 🔽 SIMPLE MODELS
record User(string Email, string Password);