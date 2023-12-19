using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThandarZinDoNetCore.ConsoleApp.Models;
using Dapper;
using System.Reflection.Metadata;

namespace ThandarZinDoNetCore.ConsoleApp.DapperExamples
{
public class DapperExampler
	{
		private readonly SqlConnectionStringBuilder _sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
		{
			DataSource = ".",
			InitialCatalog = "TestDb",
			UserID = "sa",
			Password = "sa@123",
			TrustServerCertificate = true
		};

		public void Run()
		{
			////Read();
			//Creat("Thhh", "ggg", "dddd");
			//Creat("Thhh", "ggg", "dddd");
			//Creat("Thhh", "ggg", "dddd");
			//Edit(10);
			//Delete(10);
			Update(6, "thu", "jkl", "yui");
		}

		public void Read()
		{
			string query = @"SELECT [Blog_Id]
						  ,[Blog_Title]
						  ,[Blog_Author]
						  ,[Blog_Content]
					  FROM [dbo].[Tbl_Blog]";

			using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
			List<BlogDataModel> lst = db.Query<BlogDataModel>(query).ToList();
			foreach(var item in lst)
			{
				Console.WriteLine(item.Blog_Id);
				Console.WriteLine(item.Blog_Title);
				Console.WriteLine(item.Blog_Author);
				Console.WriteLine(item.Blog_Content);

			}

		}

		public void Creat(string Title,string Author, string Content)
		{
			string query = @"INSERT INTO [dbo].[Tbl_Blog]
							   ([Blog_Title]
							   ,[Blog_Author]
							   ,[Blog_Content])
						 VALUES
							   (@Blog_Title
							   ,@Blog_Author
							   ,@Blog_Content)";

			BlogDataModel blog = new BlogDataModel()
			{
				Blog_Title = Title,
				Blog_Author = Author,
				Blog_Content = Content,
			};


			using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
			int message = db.Execute(query, blog);
			Console.WriteLine($"{message}");	
			var result = message > 0 ? "Save Successfully" : "Faild";
			Console.WriteLine(result);
		}
		private void Update(int id, string title, string author, string content)
		{
			string query = @"INSERT INTO [dbo].[Tbl_Blog]
							   ([Blog_Title]
							   ,[Blog_Author]
							   ,[Blog_Content])
						 VALUES
							   (@Blog_Title
							   ,@Blog_Author
							   ,@Blog_Content)";

			BlogDataModel blog = new BlogDataModel()
			{
				Blog_Id = id,
				Blog_Title = title,
				Blog_Author = author,
				Blog_Content = content
			};


		
			using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
			int result = db.Execute(query, blog);

			string message = result > 0 ? "Updating Successful." : "Updating Failed.";
			Console.WriteLine(message);
		}

		private void Edit(int id)
		{
			string query = @"SELECT [Blog_Id]
						  ,[Blog_Title]
						  ,[Blog_Author]
						  ,[Blog_Content]
					  FROM [dbo].[Tbl_Blog] where Blog_Id = @Blog_Id";

		

			using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
			BlogDataModel?blog = db.Query<BlogDataModel>(query,new BlogDataModel { Blog_Id = id }).FirstOrDefault();
			if (blog is null)
			{
				Console.WriteLine("No data found.");
				return;
			}
			Console.WriteLine(blog.Blog_Id);
			Console.WriteLine(blog.Blog_Title);
			Console.WriteLine(blog.Blog_Author);
			
		
		}

		private void Delete(int id)
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
			Console.WriteLine(message);

		}
	}
}
