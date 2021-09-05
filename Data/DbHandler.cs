using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AuthSystem.Models;
using Microsoft.Data.Sqlite;

namespace AuthSystem.Data
{
    public class DbHandler
    {
        private const string dbPosts = "posts";
        private const string dbComments = "comments";
        private const string dbUsers = "ASpNetUsers";
        
        public static async Task<List<Post>> GetPosts()
        {
            using SqliteConnection connection = new SqliteConnection("DataSource=app.db;Cache=Shared");
            connection.Open();
            List<Post> posts = new List<Post>();
            int tempPostID = 0;
            string tempPosterID = string.Empty;
            string tempTitle = string.Empty;
            string tempContent = string.Empty;
            string Subforum = string.Empty;
            DateTime tempDateTime = new DateTime(1630693953000);
            connection.Open();
            using var command = new SqliteCommand($"SELECT * FROM {dbPosts};", connection);
            await using var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                tempPosterID = reader.GetString(0);
                tempTitle = reader.GetString(1);
                tempContent = reader.GetString(2);
                //tempDateTime = reader.GetDateTime(3);
                Subforum = reader.GetString(4);
                tempPostID = reader.GetInt32(5);
                Post testPost = new Post(tempPostID, tempPosterID, tempTitle, tempContent, tempDateTime, Subforum);
                posts.Add(testPost);
            }
            reader.Close();
            await connection.CloseAsync();
            return posts;
        }

        public static async Task<List<Comment>> GetComments()
        {
            using SqliteConnection connection = new SqliteConnection("DataSource=app.db;Cache=Shared");
            connection.Open();
            List<Comment> output = new List<Comment>();
            int tempCommentId = 0;
            int tempPostId = 0;
            string tempText = String.Empty;
            string tempUserId = String.Empty;

            using var command = new SqliteCommand($"SELECT * FROM {dbComments};", connection);
            await using var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                tempCommentId = reader.GetInt32(0);
                tempPostId = reader.GetInt32(1);
                tempText = reader.GetString(2);
                tempUserId = reader.GetString(3);
                Comment cout = new Comment(tempCommentId, tempPostId, tempText, tempUserId);
                output.Add(cout);
            }
            connection.Close();
            return output;
        }

        public static async Task<long> GetNumberOfPosts()
        {
            using SqliteConnection connection = new SqliteConnection("DataSource=app.db;Cache=Shared");
            connection.Open();
            using var command = new SqliteCommand($"SELECT COUNT('PostId') FROM {dbPosts};", connection);
            var output = await command.ExecuteScalarAsync();
            connection.Close();
            return (long)output+1;
        }
        
        public static async Task<string> GetUserIdFromName(string email)
        {
            using SqliteConnection connection = new SqliteConnection("DataSource=app.db;Cache=Shared");
            connection.Open();
            using var command = new SqliteCommand($"SELECT Id FROM {dbUsers} WHERE Email='{email}' LIMIT 1;", connection);
            string output = (string)await command.ExecuteScalarAsync();
            connection.Close();
            return output;
        }

        public static string CleanInput(string content)
        {
            string output = content.Replace("'", "''");
            return output;
        }
        
        public static async void WritePostToDb(string posterid, string title, string content, string sub, long postId)
        {
            long postdate = DateTime.Now.Ticks;
            using SqliteConnection connection = new SqliteConnection("DataSource=app.db;Cache=Shared");
            connection.Open();
            var cleancontent = CleanInput(content);
            using var command = new SqliteCommand($"INSERT INTO {dbPosts} (PosterID, Title, Content, PostedDate, Subforum, PostId) VALUES ('{posterid}', '{title}', '{cleancontent}', {postdate}, '{sub}', {postId});", connection);
            var commandCheck = await command.ExecuteNonQueryAsync();
            if (commandCheck <= 0)
            {
                Console.WriteLine("Something went wrong with the insert query.");
                Console.WriteLine("No tables updated.");
            }
            connection.Close();
        }

        public static async void WriteCommentToDb(int postId, string content, string userId)
        {
            using var connection = new SqliteConnection("DataSource=app.db;Cache=Shared");
            connection.Open();
            var cleancontent = CleanInput(content);
            using var command = new SqliteCommand($"INSERT INTO {dbComments} (PostId, Text, UserId)"
                                                  + $"VALUES ({postId}, '{cleancontent}', '{userId}')", connection);
            var confirm = await command.ExecuteNonQueryAsync();
            if (confirm <= 0)
            {
                Debug.WriteLine("SQL-command failed, no rows changed.");
                Console.WriteLine("SQL-Command failed, no rows changed.");
            }
            connection.Close();
        }
    }
}