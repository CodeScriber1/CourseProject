using CourseProject.Data.IRepositories;
using CourseProject.Domain.Entities.Users;
using CourseProject.Service.Exceptions;
using CourseProject.Service.Extensions;
using CourseProject.Service.Interfaces.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.Service.Services.Users;

public class AuthService : IAuthService
{
	private readonly IGenericRepository<User> userRepository;
	private readonly IConfiguration configuration;
	private readonly IConfigurationSection config;

	public AuthService(IGenericRepository<User> userRepository, IConfiguration configuration)
    {
		this.userRepository = userRepository;
		this.configuration = configuration;
		this.config = configuration.GetSection("Email");
	}
   
	public async ValueTask<string> GenerateToken(string email, string password)
	{
		User user = await userRepository.GetAsync(u =>
			u.Email == email && u.Password.Equals(password.Encrypt()));

		if (user is null)
			throw new BookShopException(400, "Login or Password is incorrect");

		var authSigningKey = new SymmetricSecurityKey(
			Encoding.UTF8.GetBytes(configuration["JWT:Key"]));

		var token = new JwtSecurityToken(
			issuer: configuration["JWT:ValidIssuer"],
			expires: DateTime.Now.AddHours(int.Parse(configuration["JWT:Expire"])),
			claims: new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Role, user.UserRole.ToString())
			},
			signingCredentials: new SigningCredentials(
				key: authSigningKey,
				algorithm: SecurityAlgorithms.HmacSha256)
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}
