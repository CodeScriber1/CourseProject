namespace CourseProject.Service.DTOs.Comments;

public record CommentGetDto(int Id, int BookId, int UserId, string Text);