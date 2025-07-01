using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Comment;
using OnlineCourse.Application.RepositoryContracts;
using OnlineCourse.Domain.Entities;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class CommentService(
    IBaseRepositroy<Comment> commentRepository,
    IBaseRepositroy<Lesson> lessonRepository,
    IBaseRepositroy<User> userRepository,
    IMapper mapper) : StatusGenericHandler, ICommentService
{
    private readonly IBaseRepositroy<Comment> _commentRepository = commentRepository;
    private readonly IBaseRepositroy<Lesson> _lessonRepository = lessonRepository;
    private readonly IBaseRepositroy<User> _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateAsync(CreateCommecntModel model)
    {
        Lesson? lesson = await _lessonRepository.GetByIdAsync(model.LessonId);

        if (lesson is null)
        {
            AddError($"Lesson with id: {model.LessonId} is not found");
            return;
        }

        User? user = await _userRepository.GetByIdAsync(model.UserID);

        if (user is null)
        {
            AddError($"User with id: {model.UserID} is not found");
            return;
        }

        Comment newComment = _mapper.Map<Comment>(model);

        await _commentRepository.AddAsync(newComment);
        await _commentRepository.SaveChangesAsync();
    }

    public async Task<CommentDto?> GetByIdAsync(int id)
    {
        Comment? comment = await _commentRepository.GetByIdAsync(id);

        if (comment is null)
        {
            AddError($"Comment with id: {id} is not found");
            return null;
        }

        return _mapper.Map<CommentDto>(comment);

    }

    public async Task<IEnumerable<CommentDto>> GetByLessonAsync(int lessonId)
    {
        Lesson? lesson = await _lessonRepository.GetByIdAsync(lessonId);

        if (lesson is null)
        {
            AddError($"Lesson with id: {lessonId} is not found");
            return Enumerable.Empty<CommentDto>();
        }

        var comments = await _commentRepository.GetAll()
            .Where(x => x.LessonId == lesson.Id)
            .ToListAsync();

        return _mapper.Map<List<CommentDto>>(comments);
    }

    public async Task<IEnumerable<CommentDto>> GetRepliesAsync(int parentId)
    {
        Comment? comment = await _commentRepository.GetByIdAsync(parentId);

        if (comment is null)
        {
            AddError($"Comment with id: {parentId} is not found");
            return Enumerable.Empty<CommentDto>();
        }

        var comments = await _commentRepository.GetAll()
           .Where(x => x.Id == comment.ParentCommentId)
           .ToListAsync();

        return _mapper.Map<List<CommentDto>>(comments);
    }
}
