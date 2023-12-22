using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThandarZinDoNetCore.RestApi;
using ThandarZinDoNetCore.RestApi.EfCoreExamples;
using ThandarZinDoNetCore.RestApi.Models;

namespace ThandarZinDoNetCore.RestApi.Controllers
{
	//https://localhost:3000/api/blog
	[Route("api/[controller]")]
	[ApiController]
	public class BlogController : ControllerBase
	{
		private readonly AppDbContext _dbContext = new AppDbContext();

		[HttpGet]
		public IActionResult GetBlogs()
		{
			var lst = _dbContext.Blogs.ToList();
			return Ok(lst);

		}

        [HttpGet("{pageNo}/{pageSize}")]
        public IActionResult GetBlogs(int pageNo, int pageSize)
        {
            // pageNo = 1 [ 1 - 10]
            // pageNo = 2 [11 - 20]
            // endRowNo = pageNo * pageSize; 10
            // startRowNo = endRowNo - pageSize; 5670 - 10 = 5660 + 1 = 5661
            // 567
            var lst = _dbContext.Blogs
                .Skip((pageNo - 1) * pageSize) // 2 - 1 = 1 * 10 = 10
                .Take(pageSize)
                .ToList();
            var rowCount = _dbContext.Blogs.Count();
            var pageCount = rowCount / pageSize; // 110 / 10 = 11
            if (rowCount % pageSize > 0)
            {
                pageCount++;
            }
            return Ok(new
            {
                IsEndOfPage = pageNo >= pageCount,
                PageCount = pageCount,
                PageNo = pageNo,
                PageSize = pageSize,
                Data = lst
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetBlog(int id)
        {
            var lst = _dbContext.Blogs.FirstOrDefault(x => x.Blog_Id == id);
            if(lst is null)
            {
                return NotFound("No Data Found");
            }
            return Ok(lst);

        }

        [HttpPost]
		public IActionResult CreateBlogs(BlogDataModel blogDataModel)
		{
             _dbContext.Blogs.Add(blogDataModel);
             int result =  _dbContext.SaveChanges();
             string message = result > 0 ? "Saving Successfully" : "Save Faild";
             return Ok(message);

            
		}

		[HttpPut("{id}")]
		public IActionResult UpdateBlogs(int id , BlogDataModel blog)
		{
            var item = _dbContext.Blogs.FirstOrDefault(x => x.Blog_Id == id);
            if(item is null)
            {
                return NotFound("No Data Found");
            }

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
            item.Blog_Content = blog.Blog_Content;
            item.Blog_Author = blog.Blog_Author;
            item.Blog_Title = blog.Blog_Title;
            _dbContext.Blogs.Add(blog);
            int result = _dbContext.SaveChanges();
            string message = result > 0 ? "Updated Successfully" : "Save Faild";
            return Ok(message);
        }

        [HttpPatch("{id}")]
        public IActionResult PatchBlogs(int id, BlogDataModel blog)
        {
            var item = _dbContext.Blogs.FirstOrDefault(x => x.Blog_Id == id);
            if (item is null)
            {
                return NotFound("No data found.");
            }

            if (!string.IsNullOrEmpty(blog.Blog_Title))
            {
                item.Blog_Title = blog.Blog_Title;
            }
            if (!string.IsNullOrEmpty(blog.Blog_Author))
            {
                item.Blog_Author = blog.Blog_Author;
            }
            if (!string.IsNullOrEmpty(blog.Blog_Content))
            {
                item.Blog_Content = blog.Blog_Content;
            }

            int result = _dbContext.SaveChanges();
            string message = result > 0 ? "Updating Successful." : "Updating Failed.";

            return Ok(message);
        }

        [HttpDelete("{id}")]
		public IActionResult DeleteBlogs(int id)
		{
            var item = _dbContext.Blogs.FirstOrDefault(x => x.Blog_Id == id);
            if (item is null)
            {
                return NotFound("No data found.");
            }
            _dbContext.Blogs.Remove(item);
            int result = _dbContext.SaveChanges();
            string message = result > 0 ? "Deleting Successful." : "Deleting Failed.";

            return Ok(message);
        }
	}
}
