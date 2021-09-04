using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace AuthSystem.Models
{
    public class Post
    {
        private const string dbPosts = "posts";
        private const string dbUsers = "users";
        private const string dbComments = "comments";
        
        public int PostID;
        public string PosterID;
        public string Title;
        public string Content;
        public DateTime PostedDate;
        public string Subforum;

        public Post(int PostID, string PosterID, string Title, string Content, DateTime PostedDate, string subforum)
        {
            this.PostID = PostID;
            this.PosterID = PosterID;
            this.Title = Title;
            this.Content = Content;
            this.PostedDate = PostedDate;
            this.Subforum = subforum;
        }
        
        public string UserIdToUsername(string Id)
        {
            object? usernameOutput = "";
            SqliteConnection connection = new SqliteConnection("DataSource=app.db;Cache=Shared");
            connection.Open();
            SqliteCommand command = new SqliteCommand($"SELECT UserName FROM AspNetUsers WHERE Id='{Id}'", connection);
            command.Prepare();
            usernameOutput = command.ExecuteScalar();
            connection.CloseAsync();
            return (string)usernameOutput;
        }
    }
}