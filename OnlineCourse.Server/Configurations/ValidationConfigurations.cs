using FluentValidation;
using OnlineCourse.Application.Validators.Blog;
using OnlineCourse.Application.Validators.Category;
using OnlineCourse.Application.Validators.Comment;
using OnlineCourse.Application.Validators.Course;
using OnlineCourse.Application.Validators.Favourite;
using OnlineCourse.Application.Validators.Instructor;
using OnlineCourse.Application.Validators.Lesson;
using OnlineCourse.Application.Validators.Level;
using OnlineCourse.Application.Validators.LogValidators;
using OnlineCourse.Application.Validators.Notification;
using OnlineCourse.Application.Validators.Payment;
using OnlineCourse.Application.Validators.RefreshToken;
using OnlineCourse.Application.Validators.Review;
using OnlineCourse.Application.Validators.Student;
using OnlineCourse.Application.Validators.User;

namespace OnlineCourse.Server.Configurations;

public static class ValidationConfigurations
{
    public static void DiValidation(this WebApplicationBuilder builder)
    {
        // User
        builder.Services.AddValidatorsFromAssemblyContaining<RegisterModelValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserModelValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<LoginModelValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<ChangePasswordModelValidator>();
        // Student
        builder.Services.AddValidatorsFromAssemblyContaining<EnrollRequestModelValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<GetProgressRequestModelValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<HasCompletedRequestModelValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UpdateProgressModelValidator>();
        // Review
        builder.Services.AddValidatorsFromAssemblyContaining<CreateReviewModelValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<HasReviewedRequestModelValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UpdateReviewModelValidator>();
        // Refresh token
        builder.Services.AddValidatorsFromAssemblyContaining<RefreshTokenRequestModelValidator>();
        // Payments
        builder.Services.AddValidatorsFromAssemblyContaining<CreatePaymentModelValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<HasPaidRequestModelValidator>();
        // Notification
        builder.Services.AddValidatorsFromAssemblyContaining<CreateNotificationModelValidator>();
        // Log
        builder.Services.AddValidatorsFromAssemblyContaining<GetByTargetRequestModelValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<LogActivityRequestMdelValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<LogRequestModelValidator>();
        // Level
        builder.Services.AddValidatorsFromAssemblyContaining<CreateLevelModelValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UpdateLevelModelValidator>();
        // Lesson
        builder.Services.AddValidatorsFromAssemblyContaining<CreateLessonModelValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UpdateLessonModelValidator>();
        // Instructor
        builder.Services.AddValidatorsFromAssemblyContaining<CreateInstructorModelValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UpdateInstructorModelValidator>();
        // Favourite
        builder.Services.AddValidatorsFromAssemblyContaining<AddToFavouritesRequestModelValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<IsFavouriteRequestModelValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<RemoveFromFavouritesRequestModelValidator>();
        // Course
        builder.Services.AddValidatorsFromAssemblyContaining<CreateCourseModelValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UpdateCourseModelValidator>();
        // Comment
        builder.Services.AddValidatorsFromAssemblyContaining<CreateCommentModelValidator>();
        // Category
        builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryModelValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UpdateCategoryModelValidator>();
        // Blog
        builder.Services.AddValidatorsFromAssemblyContaining<CreateBlogValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UpdateBlogModelValidator>();




    }
}
