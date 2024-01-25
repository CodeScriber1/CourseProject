using AutoMapper;
using CourseProject.Data.IRepositories;
using CourseProject.Domain.Configurations;
using CourseProject.Domain.Entities.Users;
using CourseProject.Service.DTOs.UserDtos;
using CourseProject.Service.Exceptions;
using CourseProject.Service.Extensions;
using CourseProject.Service.Helpers;
using CourseProject.Service.Interfaces.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CourseProject.Service.Services.Users;

public class UserService : IUserService
{
    private readonly IGenericRepository<User> userRepository;
	private readonly IMapper mapper;

	public UserService(IGenericRepository<User> userRepository, IMapper mapper)
    {
		this.userRepository = userRepository;
		this.mapper = mapper;
	}

	public async ValueTask<bool> ChangePasswordAsync(UserForChangePasswordDto userForChangePasswordDTO)
    {
		var user = await userRepository.GetAsync(u => u.Id == HttpContextHelper.UserId);

		if (user == null)
			throw new BookShopException(404, "User not found");

		if (user.Password != userForChangePasswordDTO.OldPassword.Encrypt())
			throw new BookShopException(400, "Password is incorrect");


		user.Password = userForChangePasswordDTO.NewPassword.Encrypt();

		userRepository.Update(user);
		await userRepository.SaveChangesAsync();
		return true;
	}

    public async ValueTask<bool> CreateAsync(UserCreateDto userForCreationDTO)
    {
		var existEmail = await userRepository.GetAsync(u => u.Email == userForCreationDTO.Email);

		if (existEmail != null)
			throw new BookShopException(400, "This email is already taken");

		var createdUser = await userRepository.CreateAsync(mapper.Map<User>(userForCreationDTO));

		createdUser.Password = createdUser.Password.Encrypt();

		createdUser.UserRole = Domain.Enums.UserRole.User;

		await userRepository.SaveChangesAsync();

		return true;
	}

    public async ValueTask<bool> DeleteAsync(int id)
    {
        var isDeleted = await userRepository.DeleteAsync(id);

        if (!isDeleted)
            throw new BookShopException(404, "User Not Found");

        await userRepository.SaveChangesAsync();

        return true;
    }

    public async ValueTask<IEnumerable<UserGetDto>> GetAllAsync(PaginationParams @params, Expression<Func<User, bool>> expression = null)
    {
		var users = userRepository.GetAll(expression: expression, isTracking: false);
        return mapper.Map<List<UserGetDto>>(await users.ToPagedList(@params).ToListAsync());
	}

    public async ValueTask<UserGetDto> GetAsync(Expression<Func<User, bool>> expression)
    {
        var user = await userRepository.GetAsync(expression: expression);

        if (user is null)
            throw new BookShopException(404, "User not found");

        return mapper.Map<UserGetDto>(user);
    }

    public async ValueTask<UserGetDto> UpdateAsync(int id, UserUpdateDto userUpdateDto)
    {
		var existUser = await userRepository.GetAsync(
				u => u.Id == id);

		if (existUser == null)
			throw new BookShopException(404, "User not found");

		var alreadyExistUser = await userRepository.GetAsync(
			u => u.Email == userUpdateDto.Email && u.Id != HttpContextHelper.UserId);

		if (alreadyExistUser != null)
			throw new BookShopException(400, "User with such email already exists");


		existUser.UpdatedAt = DateTime.UtcNow;
		existUser = userRepository.Update(mapper.Map(userUpdateDto, existUser));
		await userRepository.SaveChangesAsync();

		return mapper.Map<UserGetDto>(existUser);
	}
}
