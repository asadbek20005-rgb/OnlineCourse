using OnlineCourse.Application.RepositoryContracts;
using OnlineCourse.Application.ServiceContracts;
using OnlineCourse.Application.ServiceContracts.Implementations;
using OnlineCourse.Infrastructure.Repositories;

namespace OnlineCourse.Server.Configurations;

public static class DependencyInjectionConfigurator
{
    public static IServiceCollection AddProjectServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepositroy<>), typeof(BaseRepository<>));
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<ISecurityService, SecurityService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ILogService, LogService>();
        services.AddScoped<ILevelService, LevelService>();
        services.AddScoped<ILessonService, LessonService>();
        services.AddScoped<IInstructorService, InstructorService>();
        services.AddScoped<IFavouriteService, FavouriteService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IActivityLogService, ActivityLogService>();
        services.AddScoped<IEmailSenderService, EmailSenderService>();
        services.AddScoped<IRedisService, RedisService>();
        services.AddScoped<IMinioService, MinioService>();
        return services;
    }
}