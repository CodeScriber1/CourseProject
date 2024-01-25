

using Microsoft.AspNetCore.Http;

namespace CourseProject.Service.DTOs.BookDtos
{
    public record BookUpdateDto(string Name, string Description, string Author, IFormFile File);
}
