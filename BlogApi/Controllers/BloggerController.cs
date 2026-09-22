using BlogApi.Models;
using BlogApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace BlogApi.Controllers
{
    [Route("blogger")]
    [ApiController]
    public class BloggerController : ControllerBase
    {
        public string ConnectionString = "server=localhost;database=blog14f;uid=root;password=;";

        [HttpGet]
        public object GetAllBlogger()
        {
            var bloggers = new List<Blogger>();

            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = "SELECT * FROM blogger;";

            var cmd = new MySqlCommand(sql, connector);

            var datareader = cmd.ExecuteReader();

            while (datareader.Read())
            {
                var blogger = new Blogger
                {
                    Id = datareader.GetInt32(0),
                    Name = datareader.GetString(1),
                    Email = datareader.GetString(2),
                    Age = datareader.GetInt32(3),
                    Password = datareader.GetString(4),
                    RegistrationTime = datareader.GetDateTime(5)
                };

                bloggers.Add(blogger);
            }

            connector.Close();


            return new
            {
                message = "Sikeres lekérdezés",
                result = bloggers
            };
        }

        [HttpGet("byId")]
        public object GetBloggerById(int id)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = @"SELECT * FROM `blogger` WHERE `id` = @id";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();

            Blogger? blogger = null;
            object? result = null;

            if (datareader.Read() == true)
            {
                blogger = new Blogger
                {
                    Id = datareader.GetInt32("id"),
                    Name = datareader.GetString("name"),
                    Email = datareader.GetString("email"),
                    Age = datareader.GetInt32("age"),
                    Password = datareader.GetString("password"),
                    RegistrationTime = datareader.GetDateTime("registrationTime")
                };

                result = new { message = "Sikeres lekérdezés", result = blogger };
            }
            else
            {
                result = new { message = "Nincs ilyen Id.", result = blogger };
            }

            connector.Close();
            return result;

        }

        [HttpPost]
        public object AddNewBlogger(AddNewBloggerDto addNewBloggerDto)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = @"INSERT INTO `blogger`(`name`, `email`, `age`, `password`, `registrationTime`) 
                VALUES (@name,@email,@age,@password,@registrationTime)";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@name", addNewBloggerDto.Name);
            cmd.Parameters.AddWithValue("@email", addNewBloggerDto.Email);
            cmd.Parameters.AddWithValue("@age", addNewBloggerDto.Age);
            cmd.Parameters.AddWithValue("@password", addNewBloggerDto.Password);
            cmd.Parameters.AddWithValue("@registrationTime", DateTime.Now);

            cmd.ExecuteNonQuery();

            connector.Close();

            return new { message = "Sikeres felvétel.", result = addNewBloggerDto };
        }

    }
}
