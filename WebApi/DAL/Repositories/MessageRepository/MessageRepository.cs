using System.Collections.Generic;
using System.Data;
using Npgsql;
using WebApi.DAL.Models;

namespace WebApi.DAL.Repositories.MessageRepository
{
    public class MessageRepository : IMessageRepository
    {
        private string _connectionString;
        private ILogger<MessageRepository> _logger;

        public MessageRepository(IConfiguration configuration, ILogger<MessageRepository> logger)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
            _connectionString = connectionString;
            _logger = logger;
        }

        public void EnsureCreated()
        {
            using (NpgsqlConnection connection = new(_connectionString))
            {
                try
                {
                    string sql = "CREATE TABLE IF NOT EXISTS Messages (Index INTEGER NOT NULL, Date TIMESTAMP NOT NULL, Text VARCHAR(128) NOT NULL);";
                    using (NpgsqlCommand command = new(sql, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        _logger.LogInformation("Ensured creation of table Messages");
                        connection.Close();
                    }
                }
                catch
                {
                    _logger.LogError("Failed to ensure creation of table Messages. Retrying...");
                    EnsureCreated();
                }
            }
        }

        public void AddMessage(ApiMessage message)
        {
            using (NpgsqlConnection connection = new(_connectionString))
            {
                string sql = "INSERT INTO Messages(Index, Date, Text) VALUES(@Index, @Date, @Text)";
                using (NpgsqlCommand command = new(sql, connection))
                {
                    connection.Open();

                    command.Parameters.AddWithValue("@Index", message.Index);
                    command.Parameters.AddWithValue("@Date", message.Date);
                    command.Parameters.AddWithValue("@Text", message.Text);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public IEnumerable<ApiMessage> GetMessages(GetMessagesModel model)
        {

            using (NpgsqlConnection connection = new(_connectionString))
            {
                string sql = "SELECT Index, Date, Text FROM Messages WHERE Date BETWEEN @StartDate AND @EndDate";
                using (NpgsqlCommand command = new(sql, connection))
                {
                    connection.Open();

                    command.Parameters.AddWithValue("@StartDate", model.StartDate);
                    command.Parameters.AddWithValue("@EndDate", model.EndDate);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new ApiMessage
                            {
                                Index = reader.GetInt32(0),
                                Date = reader.GetDateTime(1),
                                Text = reader.GetString(2)
                            };
                        }
                    }
                    connection.Close();
                }
            }
        }

    }
}
