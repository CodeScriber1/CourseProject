using CourseProject.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace CourseProject.Service.DTOs.BookDtos
{
    public record BookCreateDto(string Name, string Description, string Author, Category Category, IFormFile File);

}
