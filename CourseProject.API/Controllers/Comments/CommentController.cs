using CourseProject.Domain.Configurations;
using CourseProject.Service.DTOs.Comments;
using CourseProject.Service.Interfaces.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CourseProject.API.Controllers.Comments
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService commentService;
        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpPost]
        public async ValueTask<IActionResult> CreateCommentAsync(CommentCreateDto commentCreateDto)
            => Ok(await commentService.CreateAsync(commentCreateDto));

        [HttpDelete("{id}")]
        public async ValueTask<IActionResult> DeleteCommentAsync([FromRoute] int id)
            => Ok(await commentService.DeleteAsync(id));

        [HttpGet("{id}")]
        public async ValueTask<IActionResult> GetCommentAsync([FromRoute] int id)
            => Ok(await commentService.GetAsync(c => c.Id == id));

        [HttpGet("user/{userId}")]
        public async ValueTask<IActionResult> GetCommentByUserIdAsync([FromQuery] PaginationParams @params, [FromRoute] int userId)
            => Ok(await commentService.GetAllAsync(@params, expression: c => c.UserId == userId));

        [HttpGet("book/{bookId}")]
        public async ValueTask<IActionResult> GetCommentByBookIdAsync([FromQuery] PaginationParams @params, [FromRoute] int bookId)
            => Ok(await commentService.GetAllAsync(@params, expression: c => c.BookId == bookId));

        [HttpGet, Authorize(Roles = "Admin")]
        public async ValueTask<IActionResult> GetAllCommentsAsync([FromQuery] PaginationParams @params)
        => Ok(await commentService.GetAllAsync(@params));
    }
}
