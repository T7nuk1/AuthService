using Auth.Core;

var appBuilder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
appBuilder.Services.AddOpenApi();
S
var app = appBuilder.Build();

var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger logger = loggerFactory.CreateLogger("HttpsHooker");

Logic logic = new Logic();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/authforecast", (HttpRequest request) =>
{
    logger.LogInformation("Requested Path: {Path}", request.Path);
    return logic.CheckAuthorization(request);
})
.WithName("AuthForecast");

app.MapPut("/registrationforecast", (HttpRequest request) =>
{
    logger.LogInformation("Requested Path: {Path}", request.Path);
    return logic.CreateUser(request);
})
.WithName("RegistrationForecast");

app.Run();
