using CourseProject.Domain.Configurations;
using CourseProject.Domain.Entities.Messages;
using CourseProject.Service.DTOs.Likes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CourseProject.Service.Interfaces.Likes
{
    public interface ILikeService
    {
        ValueTask<bool> CreateAsync(LikeCreateDto likeCreateDto);
        ValueTask<bool> DeleteAsync(int id);
        ValueTask<LikeGetDto> GetAsync(Expression<Func<Like,bool>> expression);
        ValueTask<IEnumerable<LikeGetDto>> GetAllAsync(PaginationParams @params, Expression<Func<Like, bool>> expression = null);
    }
}

