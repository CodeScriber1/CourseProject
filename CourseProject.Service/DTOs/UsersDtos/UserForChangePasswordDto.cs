using System.ComponentModel.DataAnnotations;

namespace CourseProject.Service.DTOs.UserDtos;

public class UserForChangePasswordDto
{
	[Required]
	public string OldPassword { get; set; }

	[Required]
	public string NewPassword { get; set; }
}
