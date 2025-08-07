var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api", () => $"[Backend2] - {DateTime.Now}");
app.MapGet("/health", () => Results.Ok("alive"));

app.Run("http://localhost:5002");
