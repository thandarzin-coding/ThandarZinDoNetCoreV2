using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThandarZinDoNetCore.ConsoleApp.Models;

namespace ThandarZinDoNetCore.ConsoleApp.EfCoreExamples
{
	public class EfCoreExample
	{
		private readonly AppDbContext _dbContext = new AppDbContext();

		public void Run()
		{
			//Read();
			//Create("Test Title", "Test Author", "Test Content");
			//Update(12, "than", "about", "this");
			//Edit(5);
			Delete(12);
		}

		public void Read()
		{
			var lst = _dbContext.Blogs.ToList();

			foreach (var item in lst)
			{
				Console.WriteLine(item.Blog_Id);
				Console.WriteLine(item.Blog_Title);
				Console.WriteLine(item.Blog_Author);
				Console.WriteLine(item.Blog_Content);

			}
		}

		public void Create(string blogContent, string blogTitle , string blogAuthor)
		{
			BlogDataModel model = new BlogDataModel()
			{
				Blog_Author = blogAuthor,
				Blog_Content = blogContent,
				Blog_Title = blogTitle
			};
			_dbContext.Blogs.Add(model);
			int result = _dbContext.SaveChanges();

			string message = result > 0 ? "Created Successful." : "Updating Failed.";
			Console.WriteLine(message);

		}

		public void Update(int id , string blogContent, string blogTitle, string blogAuthor)
		{
			BlogDataModel? model = _dbContext.Blogs.FirstOrDefault(x => x.Blog_Id == id);
			if (model == null)
			{
				Console.WriteLine("Data Not found");
				return;
			}

			model.Blog_Author = blogAuthor;
			model.Blog_Content = blogContent;
			model.Blog_Title = blogTitle;	

			
			int result = _dbContext.SaveChanges();

			string message = result > 0 ? "Update Successful." : "Updating Failed.";
			Console.WriteLine(message);

		}

		public void Edit(int id)
		{
			BlogDataModel? item = _dbContext.Blogs.FirstOrDefault(x => x.Blog_Id==id);
			if (item == null)
			{
				Console.WriteLine("Data Not Found");
				return;
			}
			Console.WriteLine(item.Blog_Content);
			Console.WriteLine(item.Blog_Author);
			Console.WriteLine(item.Blog_Title);

			
		}

		public void Delete(int id)
		{
			BlogDataModel? item = _dbContext.Blogs.FirstOrDefault(x => x.Blog_Id == id);
			if (item == null)
			{
				Console.WriteLine("Data Not Found");
				return;
			}
			_dbContext.Blogs.Remove(item);
			int result = _dbContext.SaveChanges();
			string message = result > 0 ? "Deleting Successfully " : "Faild";
			Console.WriteLine(message);

		}
	}

}