using Auth.API;
using Microsoft.AspNetCore.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
SqlConnection.CreateConnection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/authforecast", (HttpRequest request) =>
{
    var forecast = new AuthorizationForecast(
            login: request.Query["login"],
            password: request.Query["password"]
        );
    return forecast;
})
.WithName("AuthForecast");

app.MapGet("/registrationforecast", (HttpRequest request) =>
{
    User newUser = new User(
        name: request.Query["name"],
        login: request.Query["login"],
        password: request.Query["password"],
        email: request.Query["email"]
        );
    var forecast = new CreateUserForecast(newUser);
    return forecast;
})
.WithName("RegistrationForecast");

app.Run();

record AuthorizationForecast(string login, string password)
{
    public bool SuccessState => SqlConnection.CheckAuthorization(login, password);
}

record CreateUserForecast(User newUser)
{
    public bool SuccessState => SqlConnection.CreateUser(newUser);
}
