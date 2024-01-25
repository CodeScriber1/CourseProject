using System.ComponentModel.DataAnnotations;

namespace CourseProject.Service.DTOs.UserDtos;

public record UserUpdateDto([Required] string Email);