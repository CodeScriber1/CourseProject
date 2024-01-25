namespace CourseProject.Service.DTOs.BookDtos
{
    public record BookGetDto(int Id, string Name, string Description, string Author, int Downloads = 0/*, IFormFile Fayl*/);
}
