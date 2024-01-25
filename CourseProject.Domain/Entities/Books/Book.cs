using CourseProject.Domain.Entities.Infrastructure;
using CourseProject.Domain.Entities.Messages;
using CourseProject.Domain.Entities.Users;
using CourseProject.Domain.Enums;
using System.Collections.Generic;

namespace CourseProject.Domain.Entities.Books
{
    public class Book : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public User User { get; set; }
        public string? Author { get; set; }  
        public int? UserId { get; set; }
        public Category Category { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public string FilePath { get; set; }
    }
}
