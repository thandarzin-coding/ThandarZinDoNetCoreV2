using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThandarZinDoNetCore.RestApi;
using ThandarZinDoNetCore.RestApi.EfCoreExamples;

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

		[HttpPost]
		public IActionResult CreateBlogs()
		{
			return Ok("Post");
		}

		[HttpPut]
		public IActionResult UpdateBlogs()
		{
			return Ok("Put");
		}

		[HttpPatch]
		public IActionResult PatchBlogs()
		{
			return Ok("Patch");
		}

		[HttpDelete]
		public IActionResult DeleteBlogs()
		{
			return Ok("Delete");
		}
	}
}
