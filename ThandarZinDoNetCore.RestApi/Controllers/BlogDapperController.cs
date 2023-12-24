using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata;
using ThandarZinDoNetCore.RestApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ThandarZinDoNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogDapperController : ControllerBase
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
            string query = @"SELECT [Blog_Id]
						  ,[Blog_Title]
						  ,[Blog_Author]
						  ,[Blog_Content]
					  FROM [dbo].[Tbl_Blog]";

            using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            List<BlogDataModel> lst = db.Query<BlogDataModel>(query).ToList();

            return Ok(lst);
        }


        [HttpGet("{id}")]
        public IActionResult GetBlog(int Id)
        {
            string query = @"SELECT [Blog_Id]
						  ,[Blog_Title]
						  ,[Blog_Author]
						  ,[Blog_Content]
					  FROM [dbo].[Tbl_Blog] where Blog_Id = @Blog_Id";

            using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            BlogDataModel? blog = db.Query<BlogDataModel>(query, new BlogDataModel { Blog_Id = Id }).FirstOrDefault();
            if (blog is null)
            {
                return NotFound("No Data Found");


            }
            return Ok(blog);
        }

        [HttpPost]
        public IActionResult Create(BlogDataModel blog)
        {
            string query = @"INSERT INTO [dbo].[Tbl_Blog]
							   ([Blog_Title]
							   ,[Blog_Author]
							   ,[Blog_Content])
						 VALUES
							   (@Blog_Title
							   ,@Blog_Author
							   ,@Blog_Content)";
            using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            int message = db.Execute(query, blog);

            var result = message > 0 ? "Save Successfully" : "Faild";
            return Ok(result);



        }

        [HttpPut("{id}")]
        public IActionResult UpdateBlod(int Id, BlogDataModel blog)
        {
            using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);

            #region Get By Id
            string query = @"SELECT [Blog_Id]
						  ,[Blog_Title]
						  ,[Blog_Author]
						  ,[Blog_Content]
					  FROM [dbo].[Tbl_Blog] where Blog_Id = @Blog_Id";
            #endregion

            #region Check Required Fields
            if (string.IsNullOrEmpty(blog.Blog_Title))
            {
                return BadRequest("Blog Title is required");
            }

            if (string.IsNullOrEmpty(blog.Blog_Author))
            {
                return BadRequest("Blog Author is required");
            }

            if (string.IsNullOrEmpty(blog.Blog_Content))
            {
                return BadRequest("Blog_Content is required");
            }
            #endregion

            string queryUpdate = @"UPDATE [dbo].[Tbl_Blog]
							   SET [Blog_Title] = @Blog_Title
								  ,[Blog_Author] = @Blog_Author
								  ,[Blog_Content] = @Blog_Content
							 WHERE Blog_Id = @Blog_Id";
            int result = db.Execute(queryUpdate, blog);
            string message = result > 0 ? "Updating Successful." : "Updating Failed.";
            return Ok(message);
        }


        [HttpPatch("{id}")]
        public IActionResult PatchBlog(int id, BlogDataModel blog)
        {
            using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);

            #region Get By Id

            string query = @"SELECT [Blog_Id]
      ,[Blog_Title]
      ,[Blog_Author]
      ,[Blog_Content]
  FROM [dbo].[Tbl_Blog] where Blog_Id = @Blog_Id";
            BlogDataModel? item = db.Query<BlogDataModel>(query, new BlogDataModel { Blog_Id = id }).FirstOrDefault();
            if (item is null)
            {
                return NotFound("No data found.");
            }

            #endregion

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

            #region Update

            string queryUpdate = $@"UPDATE [dbo].[Tbl_Blog]
                               SET {conditions}
                             WHERE Blog_Id = @Blog_Id";

            int result = db.Execute(queryUpdate, blog);
            string message = result > 0 ? "Updating Successful." : "Updating Failed.";

            #endregion

            return Ok(message);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id)
        {
            string query = @"DELETE FROM [dbo].[Tbl_Blog]
							  WHERE Blog_Id = @Blog_Id";

            BlogDataModel blogDataModel = new BlogDataModel()
            {
                Blog_Id = id
            };


            using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);

            int result = db.Execute(query, blogDataModel);

            string message = result > 0 ? "Deleting Successful." : "Deleting Failed.";

            return Ok(message);
        }


    }
}

