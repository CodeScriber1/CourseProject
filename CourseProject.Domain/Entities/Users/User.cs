using CourseProject.Domain.Entities.Books;
using CourseProject.Domain.Entities.Infrastructure;
using CourseProject.Domain.Entities.Messages;
using CourseProject.Domain.Enums;
using System.Collections.Generic;

namespace CourseProject.Domain.Entities.Users
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public UserRole UserRole { get; set; }
        public ICollection<Book> Books { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
    }
}
