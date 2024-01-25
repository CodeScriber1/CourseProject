using AutoMapper;
using CourseProject.Data.IRepositories;
using CourseProject.Domain.Configurations;
using CourseProject.Domain.Entities.Books;
using CourseProject.Domain.Enums;
using CourseProject.Service.DTOs.BookDtos;
using CourseProject.Service.Exceptions;
using CourseProject.Service.Extensions;
using CourseProject.Service.Helpers;
using CourseProject.Service.Interfaces.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CourseProject.Service.Services.Books
{
    public class BookService : IBookService
    {
        private readonly IGenericRepository<Book> bookRepository;
        private readonly IMapper mapper;

        public BookService(IGenericRepository<Book> bookRepository, IMapper mapper)
        {
            this.bookRepository = bookRepository;
            this.mapper = mapper;
        }
        public async ValueTask<bool> CreateAsync(BookCreateDto bookCreate)
        {
            var bookFilePath = EnvironmentHelper.BookFilePath;
            var fileName = Guid.NewGuid().ToString() + '_' + bookCreate.File.FileName;
            var filePath = Path.Combine(bookFilePath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await bookCreate.File.CopyToAsync(stream);
            }

            var newBook = mapper.Map<Book>(bookCreate);
            newBook.FilePath = filePath;
            await bookRepository.CreateAsync(newBook);
            await bookRepository.SaveChangesAsync();

            return true;
        }

        public async ValueTask<bool> DeleteAsync(int id)
        {
            var existingbook = await bookRepository.GetAsync(g => g.Id == id, false);

            if (existingbook.UserId != HttpContextHelper.UserId || HttpContextHelper.UserRole != "Admin")
                throw new BookShopException(400, "Bad Request!");

            var isDeleted = await bookRepository.DeleteAsync(id);

            if (!isDeleted)
                throw new BookShopException(404, "Book Not Found");

            await bookRepository.SaveChangesAsync();

            if (File.Exists(existingbook.FilePath))
                File.Delete(existingbook.FilePath);
            else
                throw new BookShopException(500, "File Not Deleted");

            return true;
        }

        public async ValueTask<IEnumerable<BookGetDto>> GetAllAsync(PaginationParams @params, Expression<Func<Book, bool>> expression = null)
        {
            var books = bookRepository.GetAll(expression: expression, isTracking: false);
            return mapper.Map<List<BookGetDto>>(await books.ToPagedList(@params).ToListAsync());
        }

        public async ValueTask<BookGetDto> GetAsync(Expression<Func<Book, bool>> expression)
        {
            var book = await bookRepository.GetAsync(expression);

            if (book is null)
                throw new BookShopException(404, "Book not found");

            return mapper.Map<BookGetDto>(book);
        }

        public async ValueTask<string> GetFileAsync(int id)
        {
            var existingBook = await bookRepository.GetAsync(b => b.Id == id);

            if (existingBook is null)
                throw new BookShopException(404, "Book Not Found!");

            if (!File.Exists(existingBook.FilePath))
                throw new BookShopException(404, "File Not Found!");

            string filePath = existingBook.FilePath;

            return filePath;

            //var contentType = "application/octet-stream";

            //var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            //return File(fileStream, "application/octet-stream", "staticfile.pdf");
        }

        public async ValueTask<bool> UpdateAsync(int id, BookUpdateDto bookUpdateDto)
        {
            var existingBook = await bookRepository.GetAsync(g => g.Id == id, false);

            if (existingBook is null)
                throw new BookShopException(404, "Book Not Found!");

            if (existingBook.UserId != HttpContextHelper.UserId || HttpContextHelper.UserRole != "Admin")
                throw new BookShopException(400, "Bad Request!");

            var updatingBook = mapper.Map(bookUpdateDto, existingBook);
            updatingBook.UpdatedAt = DateTime.UtcNow;

            if (bookUpdateDto.File is not null)
            {
                if (File.Exists(existingBook.FilePath))
                    File.Delete(existingBook.FilePath);

                var BookFilePath = EnvironmentHelper.BookFilePath;
                var fileName = Guid.NewGuid().ToString() + '_' + bookUpdateDto.File.FileName;
                var filePath = Path.Combine(BookFilePath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await bookUpdateDto.File.CopyToAsync(stream);
                }

                updatingBook.FilePath = filePath;
            }

            bookRepository.Update(updatingBook);

            await bookRepository.SaveChangesAsync();

            return true;
        }
    }
}
