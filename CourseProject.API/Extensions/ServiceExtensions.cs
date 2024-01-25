using CourseProject.Data.IRepositories;
using CourseProject.Data.Repositories;
using CourseProject.Domain.Entities.Books;
using CourseProject.Domain.Entities.Users;
using CourseProject.Domain.Entities.Messages;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using CourseProject.Service.Interfaces.Users;
using CourseProject.Service.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;
using CourseProject.Service.Interfaces.Books;
using CourseProject.Service.Services.Books;
using CourseProject.Service.Interfaces.Comments;
using CourseProject.Service.Services.Comments;
using CourseProject.Service.Services.Likes;
using CourseProject.Service.Interfaces.Likes;

namespace GameStoreWebApp.API.Extensions;

public static class ServiceExtensions
{
	public static void AddCustomServices(this IServiceCollection services)
	{
		// repositories
		services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
		services.AddScoped<IGenericRepository<Book>, GenericRepository<Book>>();
		services.AddScoped<IGenericRepository<Comment>, GenericRepository<Comment>>();
		services.AddScoped<IGenericRepository<Like>, GenericRepository<Like>>();

		// services
		services.AddScoped<IAuthService, AuthService>();
		services.AddScoped<IUserService, UserService>();
		services.AddScoped<IBookService, BookService>();
		services.AddScoped<ICommentService, CommentService>();
		services.AddScoped<ILikeService, LikeService>();
	}

	public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(options =>
		{
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = false,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = configuration["JWT:ValidIssuer"],
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))

			};
		});
	}

	public static void AddSwaggerService(this IServiceCollection services)
	{
		services.AddSwaggerGen(p =>
		{
			p.ResolveConflictingActions(ad => ad.First());
			p.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
			{
				Name = "Authorization",
				Type = SecuritySchemeType.ApiKey,
				BearerFormat = "JWT",
				In = ParameterLocation.Header,
			});

			p.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme()
						{
							Reference = new OpenApiReference()
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						new string[] { }
					}
				});

			p.SchemaFilter<DateOnlySchemaFilter>();
		});
	}
}

public class DateOnlySchemaFilter : ISchemaFilter
{
	public void Apply(OpenApiSchema schema, SchemaFilterContext context)
	{
		if (context.Type == typeof(DateOnly))
		{
			schema.Type = "string";
			schema.Format = "date";
		}
	}
}