using Auth.API;
using Microsoft.AspNetCore.Authentication.OAuth;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Creating Config variable
var developmentConfig = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();

// Creating MySQL connection variable
var connectionString = developmentConfig.GetSection("Data")["DefaultConnection"];
if (connectionString == null)
    return;
SqlConnection sql = new SqlConnection(connectionString);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/authforecast", (HttpRequest request) =>
{
    User authUser = new User(
        login: request.Query["login"],
        password: request.Query["password"]
        );

    var forecast = new AuthorizationForecast(
            authUser: authUser,
            successState: sql.CheckAuthorization(authUser)
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

    var forecast = new CreateUserForecast(
        newUser: newUser,
        successState: sql.CreateUser(newUser)
        );
    return forecast;
})
.WithName("RegistrationForecast");

app.Run();

record AuthorizationForecast(User authUser, bool successState)
{
}

record CreateUserForecast(User newUser, bool successState)
{
}
