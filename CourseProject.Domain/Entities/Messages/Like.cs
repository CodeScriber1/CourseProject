using CourseProject.Domain.Entities.Books;
using CourseProject.Domain.Entities.Infrastructure;
using CourseProject.Domain.Entities.Users;

namespace CourseProject.Domain.Entities.Messages
{
    public class Like : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
