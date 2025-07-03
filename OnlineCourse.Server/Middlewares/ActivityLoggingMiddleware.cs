using OnlineCourse.Domain.Entities;
using OnlineCourse.Infrastructure.Contexts;
using System.Security.Claims;

public class ActivityLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ActivityLoggingMiddleware> _logger;

    public ActivityLoggingMiddleware(RequestDelegate next, ILogger<ActivityLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, OnlineCourseDbContext dbContext)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Request pipeline-da xatolik yuz berdi.");

            throw;
        }

        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdClaim, out var userId) && userId != Guid.Empty)
            {
                string targetTableStr = "Unknown";
                string targetTableId = "";

                if (context.Request.RouteValues.TryGetValue("controller", out var controller))
                {
                    targetTableStr = controller.ToString();
                }

                if (context.Request.RouteValues.TryGetValue("id", out var id))
                {
                    targetTableId = id?.ToString() ?? "";
                }
                else
                {
                    targetTableId = context.Request.Path.ToString();
                }

                var mapping = new Dictionary<string, ActivityTargetType>(StringComparer.OrdinalIgnoreCase)
                {
                    ["users"] = ActivityTargetType.User,
                    ["students"] = ActivityTargetType.Student,
                    ["studentprogress"] = ActivityTargetType.StudentProgress,
                    ["reviews"] = ActivityTargetType.Review,
                    ["refreshtokens"] = ActivityTargetType.RefreshToken,
                    ["payments"] = ActivityTargetType.Payment,
                    ["notifications"] = ActivityTargetType.Notification,
                    ["logs"] = ActivityTargetType.Log,
                    ["levels"] = ActivityTargetType.Level,
                    ["lessons"] = ActivityTargetType.Lesson,
                    ["instructors"] = ActivityTargetType.Instructor,
                    ["favourites"] = ActivityTargetType.Favourite,
                    ["enrollments"] = ActivityTargetType.Enrollment,
                    ["courses"] = ActivityTargetType.Course,
                    ["comments"] = ActivityTargetType.Comment,
                    ["categories"] = ActivityTargetType.Category,
                };

                string controllerName = targetTableStr.ToLower();

                if (!mapping.TryGetValue(controllerName, out ActivityTargetType targetTable))
                {
                    targetTable = ActivityTargetType.User; 
                }


                var log = new ActivityLog
                {
                    UserID = userId,
                    TargetTable = targetTable,
                    TargetTableId = targetTableId
                };

                try
                {
                    dbContext.ActivityLogs.Add(log);
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception dbEx)
                {
                    // DB ga yozishda xatolik yuz berdi, uni log qilamiz
                    _logger.LogError(dbEx, "ActivityLog yozishda xatolik yuz berdi.");
                }
            }
            else
            {
                _logger.LogWarning("Foydalanuvchi IDsi topilmadi yoki noto‘g‘ri formatda.");
            }
        }
    }
}
