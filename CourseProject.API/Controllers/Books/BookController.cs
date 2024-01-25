using CourseProject.Domain.Configurations;
using CourseProject.Service.DTOs.BookDtos;
using CourseProject.Service.Interfaces.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace CourseProject.API.Controllers.Books
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService bookService;

        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        /// <summary>
        /// Add new book
        /// </summary>
        /// <param name="bookCreateDto"></param>
        /// <returns></returns>
        [HttpPost, Authorize]
        public async ValueTask<IActionResult> CreateBookAsync([FromForm] BookCreateDto bookCreateDto)
            => Ok(await bookService.CreateAsync(bookCreateDto));

        /// <summary>
        /// Update book's informations and files
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bookUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("{id}"), Authorize]
        public async ValueTask<IActionResult> UpdateBookAsync([FromRoute] int id, [FromForm] BookUpdateDto bookUpdateDto)
            => Ok(await bookService.UpdateAsync(id, bookUpdateDto));

        /// <summary>
        /// Delete all data and files related to the book
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}"), Authorize]
        public async ValueTask<IActionResult> DeleteBookAsync([FromRoute] int id)
            => Ok(await bookService.DeleteAsync(id));

        /// <summary>
        /// Get all book data by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async ValueTask<IActionResult> GetBookAsync([FromRoute] int id)
            => Ok(await bookService.GetAsync(g => g.Id == id));

        /// <summary>
        /// Get information about all books
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        [HttpGet]
        public async ValueTask<IActionResult> GetAllBooksAsync([FromQuery] PaginationParams @params)
            => Ok(await bookService.GetAllAsync(@params));

        [HttpGet("file/{fileId}")]
        public async ValueTask<IActionResult> DownloadBookFileAsync([FromRoute] int fileId)
        {
            var filePath = await bookService.GetFileAsync(fileId);

            var contentType = "application/octet-stream";

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            return File(fileStream, "application/octet-stream", "file.pdf");
        }
    }
}
