using AutoMapper;
using CourseProject.Domain.Entities.Books;
using CourseProject.Domain.Entities.Messages;
using CourseProject.Domain.Entities.Users;
using CourseProject.Service.DTOs.BookDtos;
using CourseProject.Service.DTOs.Comments;
using CourseProject.Service.DTOs.Likes;
using CourseProject.Service.DTOs.UserDtos;

namespace CourseProject.Service.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        // User
        CreateMap<User, UserGetDto>().ReverseMap();
        CreateMap<UserCreateDto, User>().ReverseMap();
        CreateMap<UserUpdateDto, User>().ReverseMap();

        // Book
        CreateMap<Book, BookGetDto>().ReverseMap();
        CreateMap<BookCreateDto, Book>().ReverseMap();
        CreateMap<BookUpdateDto, Book>().ReverseMap();

        // Comment
        CreateMap<Comment, CommentGetDto>().ReverseMap();
        CreateMap<CommentCreateDto, Comment>().ReverseMap();

        // Like
        CreateMap<Like, LikeGetDto>().ReverseMap();
        CreateMap<LikeCreateDto, Like>().ReverseMap();
    }
}
