using Microsoft.EntityFrameworkCore;
using OnlineCourse.Domain.Entities;

namespace OnlineCourse.Infrastructure.Contexts;

public class OnlineCourseDbContext : DbContext
{
    public OnlineCourseDbContext(DbContextOptions<OnlineCourseDbContext> dbContextOptions) :base(dbContextOptions)
    {
        
    }


    public DbSet<User> Users { get; set; }
    public DbSet<StudentProgress> StudentProgresses { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Log> Logs { get; set; }
    public DbSet<Level> Levels { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Favourite> Favourites { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ActivityLog> ActivityLogs { get; set; }



}
