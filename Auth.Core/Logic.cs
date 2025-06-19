using Auth.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Auth.Core
{
    public class Logic
    {
        private readonly SqlConnection sql;
        private readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        private readonly ILogger logger;
        public Logic()
        {
            sql = new SqlConnection(GetConnectionString());
            logger = loggerFactory.CreateLogger<Logic>();
        }

        private string GetConnectionString()
        {
            var developmentConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json").Build();

            var connectionString = developmentConfig.GetSection("Data")["DefaultConnection"];
            if (connectionString != null)
                return connectionString;
            return String.Empty;
        }
        public RequestAnswer CheckAuthorization(HttpRequest request)
        {

            User authUser = new User(
                login: request.Query["login"],
                password: request.Query["password"]
            );

            var answer = new RequestAnswer(
                requestBody: authUser
            );

            if (authUser.Login == null || authUser.Password == null)
            {
                logger.LogError("Couldn't get the Authentification data");
                answer.successState = false;
                return answer;
            }

            answer.successState = sql.CheckAuthorization(authUser);
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
                requestBody: newUser
            );

            if (newUser.Login == null || newUser.Password == null || newUser.Name == null
                || newUser.Email == null)
            {
                logger.LogError("Couldn't get the Authentification data");
                answer.successState = false;
                return answer;
            }

            answer.successState = sql.CheckAuthorization(newUser);
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

        public RequestAnswer(object requestBody)
        {
            this.requestBody = requestBody;
        }
    }
}
