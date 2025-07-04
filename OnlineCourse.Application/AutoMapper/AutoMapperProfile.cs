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


        CreateMap<UpdateUserModel, User>()
            .ForMember(dest => dest.FullName, opts => opts.MapFrom(x => x.FullName != null))
            .ForMember(dest => dest.UserName, opts => opts.MapFrom(x => x.UserName != null));

        CreateMap<UpdateProgressModel, StudentProgress>()
            .ForMember(dest => dest.StudentId, opts => opts.MapFrom(x => x.StudentId != null))
            .ForMember(dest => dest.CourseId, opts => opts.MapFrom(x => x.CourseId != null))
            .ForMember(dest => dest.LessonId, opts => opts.MapFrom(x => x.LessonId != null))
            .ForMember(dest => dest.ProgressPercent, opts => opts.MapFrom(x => x.ProgressPercent != null));

        CreateMap<UpdateReviewModel, Review>()
            .ForMember(dest => dest.UserID, opts => opts.MapFrom(x => x.UserID != null))
            .ForMember(dest => dest.CourseId, opts => opts.MapFrom(x => x.CourseId != null))
            .ForMember(dest => dest.Comment, opts => opts.MapFrom(x => x.Comment != null))
            .ForMember(dest => dest.Rating, opts => opts.MapFrom(x => x.Rating != null));

        CreateMap<UpdateLevelModel, Level>()
            .ForMember(dest => dest.Name, opts => opts.MapFrom(x => x.Name != null));


        CreateMap<UpdateLessonModel, Lesson>()
            .ForMember(dest => dest.CourseId, opts => opts.MapFrom(x => x.CourseId != null))
            .ForMember(dest => dest.Title, opts => opts.MapFrom(x => x.Title != null));


        CreateMap<UpdateInstructorModel, Instructor>()
            .ForMember(dest => dest.UserId, opts => opts.MapFrom(x => x.UserId != null))
            .ForMember(dest => dest.Bio, opts => opts.MapFrom(x => x.Bio != null))
            .ForMember(dest => dest.Experiance, opts => opts.MapFrom(x => x.Experiance != null));

        CreateMap<UpdateCourseModel, Course>()
            .ForMember(dest => dest.Title, opts => opts.MapFrom(x => x.Title != null))
            .ForMember(dest => dest.CategoryId, opts => opts.MapFrom(x => x.CategoryId != null))
            .ForMember(dest => dest.LevelId, opts => opts.MapFrom(x => x.LevelId != null))
            .ForMember(dest => dest.Price, opts => opts.MapFrom(x => x.Price != null));

        CreateMap<UpdateCategoryModel, Category>()
            .ForMember(dest => dest.Name, opts => opts.MapFrom(x => x.Name != null));


    }
}
