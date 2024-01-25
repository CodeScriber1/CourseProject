using CourseProject.Domain.Configurations;
using CourseProject.Service.DTOs.UserDtos;
using CourseProject.Service.Interfaces.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CourseProject.API.Controllers.Users;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService userService;

    public UserController(IUserService userService)
    {
        this.userService = userService;
    }

	/// <summary>
	/// Update user
	/// </summary>
	/// <param name="id"></param>
	/// <param name="userUpdateDto"></param>
	/// <returns></returns>
	[HttpPut("{id}")]
	public async ValueTask<IActionResult> UpdateAsync([FromRoute] int id, UserUpdateDto userUpdateDto)
        => Ok(await userService.UpdateAsync(id, userUpdateDto));

	/// <summary>
	/// Get all users
	/// </summary>
	/// <param name="params"></param>
	/// <returns></returns>
	[HttpGet, Authorize(Roles = "Admin")]
	public async ValueTask<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
        => Ok(await userService.GetAllAsync(@params));

	/// <summary>
	/// Get user
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpGet("{id}")]
	public async ValueTask<IActionResult> GetAsync([FromRoute] int id)
        => Ok(await userService.GetAsync(u => u.Id == id));

    /// <summary>
    /// User Change Password
    /// </summary>
    /// <param name="userChangePasswordDto"></param>
    /// <returns></returns>
    [HttpPatch("password")]
    public async ValueTask<IActionResult> ChangePasswordAsync(UserForChangePasswordDto userChangePasswordDto)
        => Ok(await userService.ChangePasswordAsync(userChangePasswordDto));

    /// <summary>
    /// Delete user
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}"), Authorize(Roles = "Admin")]
    public async ValueTask<IActionResult> DeleteAsync([FromRoute] int id )
        => Ok(await userService.DeleteAsync(id));
}
