using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThandarZinDoNetCore.RestApi;

namespace ThandarZinDoNetCore.RestApi.Controllers
{
	//https://localhost:3000/api/blog
	[Route("api/[controller]")]
	[ApiController]
	public class BlogController : ControllerBase
	{

		[HttpGet]
		public IActionResult GetBlogs()
		{
			return Ok("Get");
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
