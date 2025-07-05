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
    IValidator<UpdateProgressModel> updateProgressValidator
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
}
