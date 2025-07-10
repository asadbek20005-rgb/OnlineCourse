using AutoMapper;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Category;
using OnlineCourse.Application.Models.Comment;
using OnlineCourse.Application.Models.Course;
using OnlineCourse.Application.Models.Favourite;
using OnlineCourse.Application.Models.Instructor;
using OnlineCourse.Application.Models.Lesson;
using OnlineCourse.Application.Models.Level;
using OnlineCourse.Application.Models.Payment;
using OnlineCourse.Application.Models.Review;
using OnlineCourse.Application.Models.Student;
using OnlineCourse.Application.Models.User;
using OnlineCourse.Domain.Entities;

namespace OnlineCourse.Application.Mapster;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<RegisterModel, User>();
        CreateMap<EnrollRequestModel, Enrollment>();
        CreateMap<StudentProgress, StudentProgressDto>();
        CreateMap<CreateReviewModel, Review>();
        CreateMap<Review, ReviewDto>();
        CreateMap<Payment, PaymentDto>();
        CreateMap<CreatePaymentModel, Payment>();
        CreateMap<Notification, NotificationDto>();
        CreateMap<Log, LogDto>();
        CreateMap<CreateLevelModel, Level>();
        CreateMap<CreateLessonModel, Lesson>();
        CreateMap<CreateInstructorModel, Instructor>();
        CreateMap<AddToFavouriteRequestModel, Favourite>();
        CreateMap<CreateCourseModel, Course>();
        CreateMap<Course, CourseDto>();
        CreateMap<CreateCommecntModel, Comment>();
        CreateMap<Comment, CommentDto>();
        CreateMap<CreateCategoryModel, Category>();
        CreateMap<User, UserDto>();
        CreateMap<Category, CategoryDto>();
        CreateMap<Level, LevelDto>();
        CreateMap<Instructor, InstructorDto>();
        CreateMap<Favourite, FavouriteDto>();
        CreateMap<Lesson, LessonDto>();
        CreateMap<Student, StudentDto>();


        CreateMap<UpdateUserModel, User>()
            .ForMember(dest => dest.FullName, opts => opts.Condition(src => src.FullName != null))
            .ForMember(dest => dest.UserName, opts => opts.Condition(src => src.UserName != null));

        CreateMap<UpdateProgressModel, StudentProgress>()
            .ForMember(dest => dest.StudentId, opts => opts.Condition(src => src.StudentId != null))
            .ForMember(dest => dest.CourseId, opts => opts.Condition(src => src.CourseId != null))
            .ForMember(dest => dest.LessonId, opts => opts.Condition(src => src.LessonId != null))
            .ForMember(dest => dest.ProgressPercent, opts => opts.Condition(src => src.ProgressPercent != null));

        CreateMap<UpdateReviewModel, Review>()
            .ForMember(dest => dest.UserID, opts => opts.Condition(src => src.UserID != null))
            .ForMember(dest => dest.CourseId, opts => opts.Condition(src => src.CourseId != null))
            .ForMember(dest => dest.Comment, opts => opts.Condition(src => src.Comment != null))
            .ForMember(dest => dest.Rating, opts => opts.Condition(src => src.Rating != null));

        CreateMap<UpdateLevelModel, Level>()
            .ForMember(dest => dest.Name, opts => opts.Condition(src => src.Name != null));

        CreateMap<UpdateLessonModel, Lesson>()
            .ForMember(dest => dest.Title, opts => opts.Condition(src => src.Title != null));

        CreateMap<UpdateInstructorModel, Instructor>()
            .ForMember(dest => dest.UserId, opts => opts.Condition(src => src.UserId != null))
            .ForMember(dest => dest.Bio, opts => opts.Condition(src => src.Bio != null))
            .ForMember(dest => dest.Experiance, opts => opts.Condition(src => src.Experiance != null));

        CreateMap<UpdateCourseModel, Course>()
            .ForMember(dest => dest.Title, opts => opts.Condition(src => src.Title != null))
            .ForMember(dest => dest.CategoryId, opts => opts.Condition(src => src.CategoryId != null))
            .ForMember(dest => dest.LevelId, opts => opts.Condition(src => src.LevelId != null))
            .ForMember(dest => dest.Price, opts => opts.Condition(src => src.Price != null));

        CreateMap<UpdateCategoryModel, Category>()
            .ForMember(dest => dest.Name, opts => opts.Condition(src => src.Name != null));



    }
}
