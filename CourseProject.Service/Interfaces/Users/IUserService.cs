
using CourseProject.Domain.Configurations;
using CourseProject.Domain.Entities.Users;
using CourseProject.Service.DTOs.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CourseProject.Service.Interfaces.Users;

public interface IUserService
{
	ValueTask<IEnumerable<UserGetDto>> GetAllAsync(PaginationParams @params, Expression<Func<User, bool>> expression = null);
	ValueTask<UserGetDto> GetAsync(Expression<Func<User, bool>> expression);
	ValueTask<bool> CreateAsync(UserCreateDto userForCreationDTO);
	ValueTask<bool> DeleteAsync(int id);
	ValueTask<UserGetDto> UpdateAsync(int id, UserUpdateDto userForUpdateDTO);
	ValueTask<bool> ChangePasswordAsync(UserForChangePasswordDto userForChangePasswordDTO);
}
