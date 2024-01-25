using AutoMapper;
using CourseProject.Data.IRepositories;
using CourseProject.Domain.Configurations;
using CourseProject.Domain.Entities.Books;
using CourseProject.Domain.Entities.Messages;
using CourseProject.Domain.Entities.Users;
using CourseProject.Service.DTOs.Likes;
using CourseProject.Service.Exceptions;
using CourseProject.Service.Extensions;
using CourseProject.Service.Helpers;
using CourseProject.Service.Interfaces.Likes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CourseProject.Service.Services.Likes
{
    public class LikeService : ILikeService
    {
        private readonly IGenericRepository<Like> likeRepository;
        private readonly IGenericRepository<Book> bookRepository;
        private readonly IGenericRepository<User> userRepository;
        private readonly IMapper mapper;

        public LikeService(IGenericRepository<Like> likeRepository, 
            IGenericRepository<Book> bookRepository,
            IGenericRepository<User> userRepository,
            IMapper mapper)
        {
            this.likeRepository = likeRepository;
            this.bookRepository = bookRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        public async ValueTask<bool> CreateAsync(LikeCreateDto likeCreateDto)
        {
            var existUser = await userRepository.GetAsync(u => u.Id == likeCreateDto.UserId);

            if (existUser is null)
                throw new BookShopException(404, "User Not Found");

            var existBook = await bookRepository.GetAsync(b => b.Id == likeCreateDto.BookId);

            if(existBook is null)
                throw new BookShopException(404, "Book Not Found");

            await likeRepository.CreateAsync(mapper.Map<Like>(likeCreateDto));
            await likeRepository.SaveChangesAsync();

            return true;
        }

        public async ValueTask<bool> DeleteAsync(int id)
        {
            var existingLike = await likeRepository.GetAsync(l => l.Id == id);

            if (existingLike.UserId != HttpContextHelper.UserId || HttpContextHelper.UserRole != "Admin")
                throw new BookShopException(400, "Bad Request!");

            var isDeleted = await likeRepository.DeleteAsync(id);

            if (!isDeleted)
                throw new BookShopException(404, "An error occured while deleting like!");

            await likeRepository.SaveChangesAsync();
            return true;
        }

        public async ValueTask<LikeGetDto> GetAsync(Expression<Func<Like, bool>> expression)
        {
            var existingLike = await likeRepository.GetAsync(expression);

            if (existingLike is null)
                throw new BookShopException(404, "Like Not Found!");

            return mapper.Map<LikeGetDto>(existingLike);
        }

        public async ValueTask<IEnumerable<LikeGetDto>> GetAllAsync(PaginationParams @params, Expression<Func<Like, bool>> expression = null)
        {
            var likes = likeRepository.GetAll(expression: expression, isTracking: false);
            return mapper.Map<List<LikeGetDto>>(await likes.ToPagedList(@params).ToListAsync());
        }
    }
}
