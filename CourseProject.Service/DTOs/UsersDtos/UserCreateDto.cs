using System.ComponentModel.DataAnnotations;

namespace CourseProject.Service.DTOs.UserDtos;

public class UserCreateDto
{
    public string Username { get; set; } 
    public string Email { get; set; }
    [MinLength(8)]
    public string Password { get; set; }
}
