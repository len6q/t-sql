﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vegetarian_Community.Scripts
{
    public sealed class PostsCollection
    {
        private const string CONNECTION_STRING = "dbConnectionString";
        private readonly string _configConnection = ConfigurationManager.ConnectionStrings[CONNECTION_STRING].ConnectionString;

        private List<Post> _allPosts = new List<Post>();

        public int Count
        {
            get
            {
                var sqlExpression = $"SELECT COUNT(*) FROM Posts";
                using (var connection = new SqlConnection(_configConnection))
                {
                    connection.Open();
                    var command = new SqlCommand(sqlExpression, connection);
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        } 
            
        public void CreatePost(string title, int userId)
        {
            var post = new Post(Count, title, userId);
            InsertPost(post);
        }

        private async void InsertPost(Post post)
        {
            string sqlExpression = $"INSERT INTO Posts VALUES({post.Id}, '{post.Text}', {post.UserId})";
            using(var connection = new SqlConnection(_configConnection))
            {
                await connection.OpenAsync();

                var command = new SqlCommand(sqlExpression, connection);
                await command.ExecuteNonQueryAsync();
                _allPosts.Add(post);
            }
        }
    }
}
