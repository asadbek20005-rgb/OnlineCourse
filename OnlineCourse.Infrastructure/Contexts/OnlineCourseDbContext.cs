using Microsoft.EntityFrameworkCore;
using OnlineCourse.Domain.Entities;
using OnlineCourse.Infrastructure.Configurations;

namespace OnlineCourse.Infrastructure.Contexts;

public class OnlineCourseDbContext : DbContext
{
    public OnlineCourseDbContext(DbContextOptions<OnlineCourseDbContext> dbContextOptions) : base(dbContextOptions)
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



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new StudentProgressConfiguration());
        modelBuilder.ApplyConfiguration(new StudentConfiguration());
        modelBuilder.ApplyConfiguration(new ReviewConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
        modelBuilder.ApplyConfiguration(new NotificationConfiguration());
        modelBuilder.ApplyConfiguration(new LogConfiguration());
        modelBuilder.ApplyConfiguration(new LevelConfiguration());
        modelBuilder.ApplyConfiguration(new LessonConfiguration());
        modelBuilder.ApplyConfiguration(new InstructorConfiguration());
        modelBuilder.ApplyConfiguration(new FavouriteConfiguration());
        modelBuilder.ApplyConfiguration(new EnrollmentConfiguration());
        modelBuilder.ApplyConfiguration(new CourseConfiguration());
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ActivityLogConfiguration());
        modelBuilder.ApplyConfiguration(new EmailOtpConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }

}
