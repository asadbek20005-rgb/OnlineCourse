using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Student;
using OnlineCourse.Application.RepositoryContracts;
using OnlineCourse.Domain.Entities;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class StudentService(
    IBaseRepositroy<Student> studentRepository,
    IBaseRepositroy<Course> courseRepository,
    IBaseRepositroy<Enrollment> enrollmentRepository,
    IMapper mapper,
    IBaseRepositroy<StudentProgress> studentProgresssRepository,
    IValidator<EnrollRequestModel> enrollRequestValidator,
    IValidator<GetProgressRequestModel> getProgressRequestValidator,
    IValidator<HasCompletedRequestModel> hasCompletedRequestValidator,
    IValidator<UpdateProgressModel> updateProgressValidator,
    IBaseRepositroy<Instructor> _instructorRepository,
    IBaseRepositroy<User> _userRepository,
    IBaseRepositroy<Payment> _paymentRepository
    ) : StatusGenericHandler, IStudentService
{
    private readonly IBaseRepositroy<Student> _studentRepository = studentRepository;
    private readonly IBaseRepositroy<Course> _courseRepository = courseRepository;
    private readonly IBaseRepositroy<Enrollment> _enrollmentRepository = enrollmentRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IBaseRepositroy<StudentProgress> _studentProgressRepository = studentProgresssRepository;
    private readonly IValidator<EnrollRequestModel> _enrollRequestValidator = enrollRequestValidator;
    private readonly IValidator<EnrollRequestModel> _enrollValidator = enrollRequestValidator;
    private readonly IValidator<GetProgressRequestModel> _getProgressValidator = getProgressRequestValidator;
    private readonly IValidator<HasCompletedRequestModel> _completedRequest = hasCompletedRequestValidator;
    private readonly IValidator<UpdateProgressModel> _updateValidator = updateProgressValidator;


    public async Task EnrollAsync(EnrollRequestModel model)
    {
        var validatorResult = await _enrollRequestValidator.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
        }



        Student? student = await _studentRepository.GetByIdAsync(model.StudentId);
        if (student is null)
        {
            AddError($"Student with id: {model.StudentId} is not found");
            return;
        }

        Course? course = await _courseRepository.GetByIdAsync(model.CourseId);

        if (course is null)
        {
            AddError($"Course with id: {model.CourseId} is not found");
            return;
        }

        User? user = await _userRepository.GetByIdAsync(student.UserId);

        if (user is null)
        {
            AddError($"User with id: {student.UserId} is not found");
            return;
        }

        bool hasPaymentBefore = await _paymentRepository.GetAll()
         .AnyAsync(x => x.UserID == user.Id);

        if (!hasPaymentBefore)
        {
            AddError($"Student with id: {student.Id} must pay for this course with id: {course.Id}");
            return;
        }

        bool studentEnrolledBefore = await _enrollmentRepository.GetAll()
            .AnyAsync(x => x.StudentId == model.StudentId && x.CourseId == model.CourseId);

        if (studentEnrolledBefore)
        {
            AddError($"student with: id {model.StudentId} is enrolled to this course with id: {model.CourseId}");
            return;
        }

        Enrollment newEnrollment = _mapper.Map<Enrollment>(model);

        await _enrollmentRepository.AddAsync(newEnrollment);
        await _enrollmentRepository.SaveChangesAsync();
    }
    public async Task<IEnumerable<CourseDto>> GetEnrolledCourses(int id)
    {
        Student? student = await _studentRepository.GetByIdAsync(id);
        if (student is null)
        {
            AddError($"Student with id: {id} is not found");
            return new List<CourseDto>();
        }

        var myEnrollments = await _enrollmentRepository.GetAll()
            .Where(x => x.StudentId == student.Id)
            .Select(x => x.CourseId)
            .ToListAsync();
        var myCourses = new List<Course>();

        foreach (var courseId in myEnrollments)
        {
            myCourses = await _courseRepository.GetAll()
               .Where(x => x.Id == courseId)
               .ToListAsync();
        }

        return _mapper.Map<List<CourseDto>>(myCourses);
    }
    public async Task<StudentProgressDto?> GetProgressAsync(GetProgressRequestModel model)
    {

        var validatorResult = await _getProgressValidator.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
        }

        Student? student = await _studentRepository.GetByIdAsync(model.StudentId);
        if (student is null)
        {
            AddError($"Student with id: {model.StudentId} is not found");
            return null;
        }


        Course? course = await _courseRepository.GetByIdAsync(model.CourseId);

        if (course is null)
        {
            AddError($"Course with id: {model.CourseId} is not found");
            return null;
        }

        StudentProgress? studentProgress = await _studentProgressRepository.GetAll()
                                                                            .Where(x => x.StudentId == student.Id && x.CourseId == course.Id)
                                                                            .FirstOrDefaultAsync();

        if (studentProgress is null)
        {
            AddError($"Student's {student.Id} progress is not found in course {course.Id}");
            return null;
        }

        return _mapper.Map<StudentProgressDto>(studentProgress);

    }
    public async Task<StudentDto?> GetStudentByIdAsync(int id)
    {
        Student? student = await _studentRepository.GetByIdAsync(id);
        if (student is null)
        {
            AddError($"Student with id: {id} is not found");
            return null;
        }

        return _mapper.Map<StudentDto>(student);
    }
    public async Task<bool?> HasCompletedCourseAsync(HasCompletedRequestModel model)
    {

        var validatorResult = await _completedRequest.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
        }


        Student? student = await _studentRepository.GetByIdAsync(model.StudentId);
        if (student is null)
        {
            AddError($"Student with id: {model.StudentId} is not found");
            return null;
        }


        Course? course = await _courseRepository.GetByIdAsync(model.CourseId);

        if (course is null)
        {
            AddError($"Course with id: {model.CourseId} is not found");
            return null;
        }

        return course.HasCompleted;
    }
    public async Task Unenroll(UnEnrollModel model)
    {
        Student? student = await _studentRepository.GetByIdAsync(model.StudentId);
        if (student is null)
        {
            AddError($"Student with id: {model.StudentId} is not found");
            return;
        }


        Course? course = await _courseRepository.GetByIdAsync(model.CourseId);

        if (course is null)
        {
            AddError($"Course with id: {model.CourseId} is not found");
            return;
        }

        Enrollment? enrollment = await _enrollmentRepository.GetByIdAsync(model.EnrollmentId);
        if (enrollment is null)
        {
            AddError($"Enrollment with id: {model.EnrollmentId} is not found");
            return;
        }


        await _enrollmentRepository.DeleteAsync(enrollment);
        await _enrollmentRepository.SaveChangesAsync();

    }
    public async Task UpdateProgressAsync(UpdateProgressModel model)
    {
        var validatorResult = await _updateValidator.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
        }


        Student? student = await _studentRepository.GetByIdAsync(model.StudentId);
        if (student is null)
        {
            AddError($"Student with id: {model.StudentId} is not found");
            return;
        }


        Course? course = await _courseRepository.GetByIdAsync(model.CourseId);

        if (course is null)
        {
            AddError($"Course with id: {model.CourseId} is not found");
            return;
        }

        StudentProgress? studentProgress = await _studentProgressRepository.GetAll()
                                          .Where(x => x.StudentId == student.Id && x.CourseId == course.Id)
                                          .FirstOrDefaultAsync();

        if (studentProgress is null)
        {
            AddError($"Student's {student.Id} progress is not found in course {course.Id}");
            return;
        }

        StudentProgress updatedProgress = _mapper.Map(model, studentProgress);

        await _studentProgressRepository.UpdateAsync(updatedProgress);
        await _courseRepository.SaveChangesAsync();
    }
    public async Task<IEnumerable<LessonDto>> GetLessonsByCourseIdAsync(GetLessonsByCourseRequestModel model)
    {
        Student? student = await _studentRepository.GetByIdAsync(model.StudentId);
        if (student is null)
        {
            AddError($"Student with id: {model.StudentId} is not found");
            return Enumerable.Empty<LessonDto>();
        }

        Course? course = await _courseRepository.GetByIdAsync(model.CourseId);
        if (course is null)
        {
            AddError($"Course with id: {model.CourseId} is not found");
            return Enumerable.Empty<LessonDto>();
        }

        bool isEnrolled = await _enrollmentRepository.GetAll()
            .AnyAsync(e => e.StudentId == model.StudentId && e.CourseId == model.CourseId);

        if (!isEnrolled)
        {
            AddError("Student is not enrolled in this course.");
            return Enumerable.Empty<LessonDto>();
        }

        var lessons = course.Lessons ?? [];

        return _mapper.Map<IEnumerable<LessonDto>>(lessons);
    }
    public async Task<LessonDto?> GetLessonByIdAsync(GetLessonByIdRequestModel model)
    {
        Student? student = await _studentRepository.GetByIdAsync(model.StudentId);
        if (student is null)
        {
            AddError($"Student with id: {model.StudentId} is not found");
            return null;
        }

        Course? course = await _courseRepository.GetAll()
            .Include(c => c.Lessons)
            .FirstOrDefaultAsync(c => c.Id == model.CourseId);

        if (course is null)
        {
            AddError($"Course with id: {model.CourseId} is not found");
            return null;
        }

        bool isEnrolled = await _enrollmentRepository.GetAll()
            .AnyAsync(e => e.StudentId == model.StudentId && e.CourseId == model.CourseId);

        if (!isEnrolled)
        {
            AddError($"Student with id: {student.Id} is not enrolled in this course with id: {course.Id}.");
            return null;
        }

        Lesson? lesson = course.Lessons?.FirstOrDefault(l => l.Id == model.LessonId);

        if (lesson is null)
        {
            AddError($"Lesson with id: {model.LessonId} is not found");
            return null;
        }

        return _mapper.Map<LessonDto>(lesson);
    }
    public async Task<IEnumerable<CourseDto>> GetFavoriteCoursesAsync(int studentId)
    {
        Student? student = await _studentRepository.GetByIdAsync(studentId);
        if (student is null)
        {
            AddError($"Student with id: {studentId} is not found");
            return Enumerable.Empty<CourseDto>();
        }

        var courses = await _enrollmentRepository.GetAll()
       .Where(e => e.StudentId == studentId)
       .Include(e => e.Course)
       .Select(e => e.Course)
       .ToListAsync();


        return _mapper.Map<List<CourseDto>>(courses);
    }
    public async Task<int> GetActiveStudentCountAsync()
    {
        return await _studentRepository.GetAll()
            .CountAsync();
    }
    public async Task<int> GetStudentsCountByInstructorIdAsync(GetInstructorStudentsCount model)
    {
        Instructor? instructor = await _instructorRepository.GetByIdAsync(model.InstructorId);

        if (instructor is null)
        {
            AddError($"Instructor with id: {model.InstructorId} is not found");
            return 0;
        }

        return await _enrollmentRepository.GetAll()
      .Where(e => e.Course!.InstructorId == instructor.Id)
      .CountAsync();
    }
    public async Task<int> GetTotalStudentsCountByCourseId(GetStudentsByCourseIdModel model)
    {
        Course? course = await _courseRepository.GetByIdAsync(model.CourseId);

        if (course is null)
        {
            AddError($"Course with id: {model.CourseId} is not found");
            return 0;
        }

        return await _enrollmentRepository.GetAll()
            .Where(x => x.CourseId == course.Id)
            .CountAsync();
    }

    public async Task CreateAsync(CreateStudentModel model)
    {
        bool studentExist = await _studentRepository.GetAll()
            .AnyAsync(x => x.UserId == model.UserId);

        if (studentExist)
        {
            AddError($"Student with user id: {model.UserId} is already exist");
            return;
        }


        User? user = await _userRepository.GetByIdAsync(model.UserId);

        if (user is null)
        {
            AddError($"User with id: {model.UserId} is not found");
            return;
        }

        var newStudent = new Student
        {
            UserId = user.Id
        };

        await _studentRepository.AddAsync(newStudent);
        await _studentRepository.SaveChangesAsync();
    }
}