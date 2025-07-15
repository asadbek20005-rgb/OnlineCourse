using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Blog;
using OnlineCourse.Application.Models.Pagination;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts;

public interface IBlogService : IStatusGeneric
{
    Task CreateAsync(Guid userId,CreateBlogModel model);
    Task<IEnumerable<BlogDto>> GetAllAsync();
    Task<IEnumerable<BlogDto>> GetBlogsByUserIdAsync(Guid userId);
    Task<IEnumerable<BlogDto>> GetBlogsByPaginationAsync(PaginationModel model);
    Task<BlogDto?> GetBlogByUserIdAsync(Guid userId, int blogId);
    Task UpdateAsync(Guid userId, int blogId,UpdateBlogModel model);
    Task DeleteAsync(Guid userId, int blogId);
    Task ExistAsync(Guid userId, int blogId);
}
