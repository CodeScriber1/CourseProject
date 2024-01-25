using CourseProject.Domain.Configurations;
using CourseProject.Domain.Entities.Messages;
using CourseProject.Service.DTOs.Comments;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;

namespace CourseProject.Service.Interfaces.Comments
{
    public interface ICommentService
    {
        public  ValueTask<bool> CreateAsync(CommentCreateDto CommentCreateDto);
        public ValueTask<IEnumerable<CommentGetDto>> GetAllAsync(PaginationParams @params, Expression<Func<Comment, bool>> expression = null);
        public ValueTask<CommentGetDto> GetAsync(Expression<Func<Comment, bool>> expression);
        public ValueTask<bool> DeleteAsync(int id);
    }
}
