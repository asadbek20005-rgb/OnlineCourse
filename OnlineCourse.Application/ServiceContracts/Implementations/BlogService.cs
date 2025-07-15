using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Blog;
using OnlineCourse.Application.Models.Pagination;
using OnlineCourse.Application.RepositoryContracts;
using OnlineCourse.Domain.Entities;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class BlogService(
    IBaseRepositroy<Blog> _blogRepository,
    IBaseRepositroy<User> _userRepository,
    IMapper _mapper) : StatusGenericHandler, IBlogService
{
    public async Task CreateAsync(Guid userId, CreateBlogModel model)
    {
        User? user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            AddError($"User with id: {userId} is not found");
            return;
        }

        Blog newBlog = _mapper.Map<Blog>(model);
        newBlog.UserId = user.Id;
        await _blogRepository.AddAsync(newBlog);
        await _blogRepository.SaveChangesAsync();

    }

    public async Task DeleteAsync(Guid userId, int blogId)
    {
        User? user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            AddError($"User with id: {userId} is not found");
            return;
        }

        Blog? blog = await _blogRepository.GetAll()
            .Where(x => x.UserId == user.Id && x.Id == blogId)
            .FirstOrDefaultAsync();

        if (blog is null)
        {
            AddError($"Blog with id: {blogId} is not found");
            return;
        }

        await _blogRepository.DeleteAsync(blog);
        await _blogRepository.SaveChangesAsync();
    }

    public Task ExistAsync(Guid userId, int blogId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<BlogDto>> GetAllAsync()
    {
        var blogs = await _blogRepository.GetAll().ToListAsync();

        return _mapper.Map<List<BlogDto>>(blogs);
    }

    public async Task<BlogDto?> GetBlogByUserIdAsync(Guid userId, int blogId)
    {
        User? user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            AddError($"User with id: {userId} is not found");
            return null;
        }

        Blog? blog = await _blogRepository.GetAll()
            .Where(x => x.UserId == user.Id && x.Id == blogId)
            .FirstOrDefaultAsync();

        if (blog is null)
        {
            AddError($"Blog with id: {blogId} is not found");
            return null;
        }

        return _mapper.Map<BlogDto>(blog);
    }

    public async Task<IEnumerable<BlogDto>> GetBlogsByPaginationAsync(PaginationModel model)
    {
        var query = _blogRepository.GetAll();

        var blogs = await query.Skip((model.Page - 1) * model.PageSize).Take(model.PageSize).ToListAsync();

        return _mapper.Map<List<BlogDto>>(blogs);
    }

    public async Task<IEnumerable<BlogDto>> GetBlogsByUserIdAsync(Guid userId)
    {
        User? user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            AddError($"User with id: {userId} is not found");
            return Enumerable.Empty<BlogDto>();
        }

        var blogs = await _blogRepository.GetAll()
            .Where(x => x.UserId == user.Id)
            .ToListAsync();

        return _mapper.Map<List<BlogDto>>(blogs);
    }

    public async Task UpdateAsync(Guid userId, int blogId, UpdateBlogModel model)
    {
        User? user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            AddError($"User with id: {userId} is not found");
            return;
        }

        Blog? blog = await _blogRepository.GetAll()
            .Where(x => x.UserId == user.Id && x.Id == blogId)
            .FirstOrDefaultAsync();


        if (blog is null)
        {
            AddError($"Blog with id: {blogId} is not found");
            return;
        }

        Blog updated = _mapper.Map(model, blog);


        await _blogRepository.UpdateAsync(updated);
        await _blogRepository.SaveChangesAsync();

    }
}
