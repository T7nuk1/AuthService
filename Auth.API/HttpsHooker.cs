using Auth.Core;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

Logic logic = new Logic();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/authforecast", (HttpRequest request) =>
{
    return logic.CheckAuthorization(request);
})
.WithName("AuthForecast");

app.MapPut("/registrationforecast", (HttpRequest request) =>
{
    return logic.CreateUser(request);
})
.WithName("RegistrationForecast");

app.Run();
