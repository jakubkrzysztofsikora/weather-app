WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
WebApplication app = builder.Build();

string port = Environment.GetEnvironmentVariable("PORT") ?? "3001";

app.MapGet("/", () => "Hello World!");

app.Run($"http://*:{port}");
