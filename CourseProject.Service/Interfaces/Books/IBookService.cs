using CourseProject.Domain.Configurations;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using CourseProject.Domain.Entities.Books;
using CourseProject.Service.DTOs.BookDtos;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace CourseProject.Service.Interfaces.Books;

public interface IBookService
{
    ValueTask<bool> CreateAsync(BookCreateDto bookCreate);
    ValueTask<bool> UpdateAsync(int id, BookUpdateDto bookUpdate);
    ValueTask<bool> DeleteAsync(int id);
    ValueTask<BookGetDto> GetAsync(Expression<Func<Book, bool>> expression);
    ValueTask<IEnumerable<BookGetDto>> GetAllAsync(PaginationParams @params, Expression<Func<Book, bool>> expression = null);
    ValueTask<string> GetFileAsync(int id);
}
