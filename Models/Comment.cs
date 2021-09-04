using System.Security.Permissions;
using Microsoft.Data.Sqlite;

namespace AuthSystem.Models
{
    public class Comment
    {
        private const string dbPosts = "posts";
        private const string dbUsers = "AspNetUsers";
        private const string dbComments = "comments";

        public int CommentId;
        public int PostId;
        public string Text;
        public string UserId;

        public Comment(int commentId, int postId, string text, string userId)
        {
            this.CommentId = commentId;
            this.PostId = postId;
            this.Text = text;
            this.UserId = userId;
        }

        public string UserIdToUsername(string userId)
        {
            using SqliteConnection connection = new SqliteConnection("DataSource=app.db;Cache=Shared");
            connection.Open();
            using var command = new SqliteCommand($"SELECT Username FROM {dbUsers} WHERE ID='{userId}';", connection);
            command.Prepare();
            object? usernameOutput = command.ExecuteScalar();
            connection.CloseAsync();
            return (string)usernameOutput;
        }
    }
}