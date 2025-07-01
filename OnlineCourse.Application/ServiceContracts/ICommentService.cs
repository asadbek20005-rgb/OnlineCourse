using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Comment;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts;

public interface ICommentService : IStatusGeneric
{
    Task<CommentDto?> GetByIdAsync(int id);
    Task<IEnumerable<CommentDto>> GetByLessonAsync(int lessonId);
    Task CreateAsync(CreateCommecntModel model);
    Task<IEnumerable<CommentDto>> GetRepliesAsync(int parentId);

}
