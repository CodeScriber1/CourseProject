namespace CourseProject.Service.DTOs.Comments;

public record CommentCreateDto(int UserId, int BookId, string Text);