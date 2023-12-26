using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ThandarZinDoNetCore.RestApi.Models;


namespace ThandarZinDoNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogAdoDoNetController : ControllerBase
    {
        private readonly SqlConnectionStringBuilder _sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
        {
            DataSource = ".",
            InitialCatalog = "TestDb",
            UserID = "sa",
            Password = "sa@123",
            TrustServerCertificate = true
        };

        [HttpGet]
        public IActionResult GetBlogs()
        {
            SqlConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            string query = @"SELECT [Blog_Id]
                            ,[Blog_Title]
                            ,[Blog_Author]
                            ,[Blog_Content]
                            FROM [dbo].[Tbl_Blog]";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sqlDataAdapter.Fill(dt);

            connection.Close();


            List<BlogDataModel> lst = dt.AsEnumerable().Select(dr => new BlogDataModel
            {
                Blog_Id = Convert.ToInt32(dr["Blog_Id"]),
                Blog_Title = Convert.ToString(dr["Blog_Title"]),
                Blog_Author = Convert.ToString(dr["Blog_Author"]),
                Blog_Content = Convert.ToString(dr["Blog_Content"]),
            }).ToList();

            return Ok(lst);
        }

        [HttpGet("{id}")]
        public IActionResult GetBlog(int id)
        {
            SqlConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            string query = @"SELECT [Blog_Id]
                            ,[Blog_Title]
                            ,[Blog_Author]
                            ,[Blog_Content]
                            FROM [dbo].[Tbl_Blog] where Blog_Id = @Blog_Id ";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Blog_Id", id);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sqlDataAdapter.Fill(dt);

            connection.Close();


            BlogDataModel? blogDataModel = dt.AsEnumerable().Select(dr => new BlogDataModel
            {
                Blog_Id = Convert.ToInt32(dr["Blog_Id"]),
                Blog_Title = Convert.ToString(dr["Blog_Title"]),
                Blog_Author = Convert.ToString(dr["Blog_Author"]),
                Blog_Content = Convert.ToString(dr["Blog_Content"]),
            }).FirstOrDefault();

            return Ok(blogDataModel);
        }

        [HttpPost]
        public IActionResult Create(BlogDataModel blog)
        {
            SqlConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            string query = @"INSERT INTO [dbo].[Tbl_Blog]
							   ([Blog_Title]
							   ,[Blog_Author]
							   ,[Blog_Content])
						 VALUES
							   (@Blog_Title
							   ,@Blog_Author
							   ,@Blog_Content)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Blog_Title", blog.Blog_Title);
            command.Parameters.AddWithValue("@Blog_Content", blog.Blog_Content);
            command.Parameters.AddWithValue("@Blog_Author", blog.Blog_Author);
            var result = command.ExecuteNonQuery();
            connection.Close();
            string message = result > 0 ? " Created Successfully " : "Saving Faild";
            return Ok(message);

        }

        [HttpPut("{id}")]
        public IActionResult UpdateBlog(int Id, BlogDataModel blog)
        {
            SqlConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            string query = @"UPDATE [dbo].[Tbl_Blog]
							   SET [Blog_Title] = @Blog_Title
								  ,[Blog_Author] = @Blog_Author
								  ,[Blog_Content] = @Blog_Content
							 WHERE Blog_Id = @Blog_Id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Blog_Id", Id);
            command.Parameters.AddWithValue("@Blog_Title", blog.Blog_Title);
            command.Parameters.AddWithValue("@Blog_Content", blog.Blog_Content);
            command.Parameters.AddWithValue("@Blog_Author", blog.Blog_Author);
            var result = command.ExecuteNonQuery();
            connection.Close();

            string message = result > 0 ? " Updated successfully" : "Updateed Faild";
            return Ok(message); ;

        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id)
        {
            SqlConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            string query = @"DELETE FROM [dbo].[Tbl_Blog]
						   WHERE Blog_Id = @Blog_Id";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Blog_Id", id);
            var result = command.ExecuteNonQuery();
            connection.Close();

            string message = result > 0 ? "Deleted successfully" : "Delete Faild";
            return Ok(message);
        }

        [HttpPatch("{id}")]
        public IActionResult PatchBlog(int id, BlogDataModel blog)
        {
            SqlConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            string query = @"SELECT [Blog_Id]
                            ,[Blog_Title]
                            ,[Blog_Author]
                            ,[Blog_Content]
                            FROM [dbo].[Tbl_Blog] where Blog_Id = @Blog_Id ";


            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Blog_Id", id);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sqlDataAdapter.Fill(dt);

            connection.Close();

            if (dt is null)
            {
                return NotFound("No data found.");
            }

            string conditions = string.Empty;

            if (!string.IsNullOrEmpty(blog.Blog_Title))
            {
                conditions += @" [Blog_Title] = @Blog_Title, ";
            }
            if (!string.IsNullOrEmpty(blog.Blog_Author))
            {
                conditions += @" [Blog_Author] = @Blog_Author, ";
            }
            if (!string.IsNullOrEmpty(blog.Blog_Content))
            {
                conditions += @" [Blog_Content] = @Blog_Content, ";
            }
            if (conditions.Length == 0)
            {
                return BadRequest("Invalid Request.");
            }
            conditions = conditions.Substring(0, conditions.Length - 2);

            blog.Blog_Id = id;

            string queryUpdate = $@"UPDATE [dbo].[Tbl_Blog]
                               SET {conditions}
                             WHERE Blog_Id = @Blog_Id";

            connection.Open();
            SqlCommand cmdUpdate = new SqlCommand(queryUpdate, connection);
            cmdUpdate.Parameters.AddWithValue("@Blog_Id", id);
            if (!string.IsNullOrEmpty(blog.Blog_Title))
            {
                cmdUpdate.Parameters.AddWithValue("@Blog_Title", blog.Blog_Title);
            }
            if (!string.IsNullOrEmpty(blog.Blog_Author))
            {
                cmdUpdate.Parameters.AddWithValue("@Blog_Author", blog.Blog_Author);
            }
            if (!string.IsNullOrEmpty(blog.Blog_Content))
            {
                cmdUpdate.Parameters.AddWithValue("@Blog_Content", blog.Blog_Content);
            }

            var result = cmdUpdate.ExecuteNonQuery();
            connection.Close();

            string message = result > 0 ? "Update successfully" : " Faild";
            return Ok(message);

        }
    }
}
