using MySqlConnector;

namespace Auth.Infrastructure
{
    public class SqlConnection
    {
        private MySqlConnection _connection;
        public SqlConnection(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
            _connection.Open();
        }

        public bool CheckAuthorization(User user)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM users WHERE login = @login AND password = @password";
            command.Parameters.AddWithValue("login", user.Login);
            command.Parameters.AddWithValue("password", user.Password);

            var result = command.ExecuteScalar();
            if (result != null && (long)result > 0)
                return true;
            else
                return false;
        }

        public bool CreateUser(User newUser)
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
        public string? Name { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }

        public User(string name, string login, string password,
            string email)
        {
            Name = name;
            Login = login;
            Password = password;
            Email = email;
        }

        public User(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
