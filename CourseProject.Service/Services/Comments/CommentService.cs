using AutoMapper;
using CourseProject.Data.IRepositories;
using CourseProject.Domain.Configurations;
using CourseProject.Domain.Entities.Messages;
using CourseProject.Service.DTOs.Comments;
using CourseProject.Service.Exceptions;
using CourseProject.Service.Extensions;
using CourseProject.Service.Helpers;
using CourseProject.Service.Interfaces.Comments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CourseProject.Service.Services.Comments;

public class CommentService : ICommentService
{
	private readonly IGenericRepository<Comment> commentRepository;
	private readonly IMapper mapper;

	public CommentService(IGenericRepository<Comment> commentRepository, IMapper mapper)
	{
		this.commentRepository = commentRepository;
		this.mapper = mapper;
	}

	public async ValueTask<bool> CreateAsync(CommentCreateDto CommentCreateDto)
	{
		var creatingComment = await commentRepository.CreateAsync(mapper.Map<Comment>(CommentCreateDto));
		await commentRepository.SaveChangesAsync();

		return true;
	}

    public async ValueTask<bool> DeleteAsync(int id)
    {
		var existingComment = await commentRepository.GetAsync(c => c.Id == id);

        if (existingComment.UserId != HttpContextHelper.UserId || HttpContextHelper.UserRole != "Admin")
            throw new BookShopException(400, "Bad Request!");

        var isDeleted = await commentRepository.DeleteAsync(id);

        if (!isDeleted)
            throw new BookShopException(404, "Rate Not Found");

        await commentRepository.SaveChangesAsync();

        return true;
    }

    public async ValueTask<IEnumerable<CommentGetDto>> GetAllAsync(PaginationParams @params, Expression<Func<Comment, bool>> expression = null)
	{
		var Comments = commentRepository.GetAll(expression: expression, isTracking: false);
		return mapper.Map<IEnumerable<CommentGetDto>>(await Comments.ToPagedList(@params).ToListAsync());
	}

	public async ValueTask<CommentGetDto> GetAsync(Expression<Func<Comment, bool>> expression)
	{
		var Comment = await commentRepository.GetAsync(expression);

		if (Comment is null)
			throw new BookShopException(404, "Comment not found");

		return mapper.Map<CommentGetDto>(Comment);
	}
}
