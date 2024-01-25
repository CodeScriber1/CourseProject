using CourseProject.Domain.Configurations;
using CourseProject.Service.DTOs.Likes;
using CourseProject.Service.Interfaces.Likes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CourseProject.API.Controllers.Likes
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService likeService;

        public LikeController(ILikeService likeService)
        {
            this.likeService = likeService;
        }

        [HttpPost]
        public async ValueTask<IActionResult> CreateLikeAsync(LikeCreateDto likeCreateDto)
            => Ok(await likeService.CreateAsync(likeCreateDto));

        [HttpDelete("{id}")]
        public async ValueTask<IActionResult> DeleteLikeAsync([FromRoute] int id)
            => Ok(await likeService.DeleteAsync(id));
        
        [HttpGet]
        public async ValueTask<IActionResult> GetAllLikesAsync([FromQuery] PaginationParams @params)
            => Ok(await likeService.GetAllAsync(@params));

        [HttpGet("{id}")]
        public async ValueTask<IActionResult> GetLikeAsync([FromRoute] int id)
            => Ok(await likeService.GetAsync(l => l.Id == id));

        [HttpGet("book/{bookId}")]
        public async ValueTask<IActionResult> GetLikesByBookIdAsync([FromQuery] PaginationParams @params, [FromRoute] int bookId)
            => Ok(await likeService.GetAllAsync(@params, expression: l => l.BookId == bookId));
    }
}
