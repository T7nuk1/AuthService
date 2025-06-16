using MySqlConnector;

namespace Auth.API
{
    public class SqlConnection
    {
        private static MySqlConnection _connection;
        public static void CreateConnection()
        {
            _connection = new MySqlConnection("Server=localhost;User ID=root;Password=admin;Database=mservices");
            _connection.Open();
        }

        public static bool CheckAuthorization(string login, string password)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM users WHERE login = @login AND password = @password";
            command.Parameters.AddWithValue("login", login);
            command.Parameters.AddWithValue("password", password);

            if ((long)command.ExecuteScalar() > 0)
                return true;
            else
                return false;
        }

        public static bool CreateUser(User newUser)
        {
            var command = _connection.CreateCommand();
            command.CommandText = @"INSERT INTO users (name, login, password, email) VALUES
                    (@name, @login, @password, @email)";
            command.Parameters.AddWithValue("@name", newUser.Name);
            command.Parameters.AddWithValue("@login", newUser.Login);
            command.Parameters.AddWithValue("@password", newUser.Password);
            command.Parameters.AddWithValue("@email", newUser.Email);

            if (command.ExecuteNonQuery() > 0)
                return true;
            else
                return false;
        }
    }

    public class User
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public User(string name, string login, string password,
            string email)
        {
            Name = name;
            Login = login;
            Password = password;
            Email = email;
        }
    }
}
