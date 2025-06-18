using Auth.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Auth.Core
{
    public class Logic
    {
        SqlConnection sql;
        public Logic()
        {
            var developmentConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json").Build();

            var connectionString = developmentConfig.GetSection("Data")["DefaultConnection"];
            if (connectionString == null)
                return;
            sql = new SqlConnection(connectionString);
        }
        public RequestAnswer CheckAuthorization(HttpRequest request)
        {
            User authUser = new User(
                login: request.Query["login"],
                password: request.Query["password"]
            );

            var answer = new RequestAnswer(
                requestBody: authUser,
                successState: sql.CheckAuthorization(authUser)
            );
            return answer;
        }

        public RequestAnswer CreateUser(HttpRequest request)
        {
            User newUser = new User(
                name: request.Query["name"],
                login: request.Query["login"],
                password: request.Query["password"],
                email: request.Query["email"]
            );

            var answer = new RequestAnswer(
                requestBody: newUser,
                successState: sql.CreateUser(newUser)
            );
            return answer;
        }
    }

    public record RequestAnswer
    {
        public object? requestBody { get; set; }
        public bool successState { get; set; }

        public RequestAnswer(object requestBody, bool successState)
        {
            this.requestBody = requestBody;
            this.successState = successState;
        }
    }
}
