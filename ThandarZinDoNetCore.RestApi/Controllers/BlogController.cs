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
