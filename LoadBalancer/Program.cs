using System.Net;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<string> backends = new()
{
    "http://localhost:5001",
    "http://localhost:5002",
    "http://localhost:5003"
};

List<string> healthyBackends = new(backends);
int currentIndex = 0;
HttpClient client = new();

var timer = new System.Timers.Timer(5000);
timer.Elapsed += async (sender, e) =>
{
    List<string> alive = new();
    foreach (var backend in backends)
    {
        try
        {
            var response = await client.GetAsync($"{backend}/health");
            if (response.IsSuccessStatusCode)
                alive.Add(backend);
        }
        catch { }
    }

    lock (healthyBackends)
    {
        healthyBackends.Clear();
        healthyBackends.AddRange(alive);
    }
};
timer.Start();

app.MapGet("/proxy", async () =>
{
    string backend;
    lock (healthyBackends)
    {
        if (healthyBackends.Count == 0)
            return Results.Problem("Nenhum servidor disponível", statusCode: 503);

        if (currentIndex >= healthyBackends.Count)
            currentIndex = 0;

        backend = healthyBackends[currentIndex];
        currentIndex = (currentIndex + 1) % healthyBackends.Count;
    }

    try
    {
        var response = await client.GetAsync($"{backend}/api");
        var body = await response.Content.ReadAsStringAsync();
        return Results.Content(body, "application/json");
    }
    catch
    {
        return Results.Problem("Erro ao conectar ao backend", statusCode: 502);
    }
});

app.Run("http://localhost:5000");
