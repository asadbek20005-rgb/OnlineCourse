using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Application.Extensions;
using OnlineCourse.Application.Models.Student;
using OnlineCourse.Application.ServiceContracts;

namespace OnlineCourse.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize("Student")]
public class StudentsController(IStudentService _studentService) : ControllerBase
{
    [HttpGet("student-id")]
    public async Task<IActionResult> GetById(int id)
    {
        var student = await _studentService.GetStudentByIdAsync(id);

        if (_studentService.IsValid)
            return Ok(student);

        _studentService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpGet("student-id/courses")]
    public async Task<IActionResult> GetCourses(int id)
    {
        var courses = await _studentService.GetEnrolledCourses(id);
        if (_studentService.IsValid)
            return Ok(courses);

        _studentService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpPost("student-id/Courses/course-id/Lessons")]
    public async Task<IActionResult> GetLessons(GetLessonsByCourseRequestModel model)
    {
        var lessons = await _studentService.GetLessonsByCourseIdAsync(model);

        if (_studentService.IsValid)
            return Ok(lessons);

        _studentService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpPost("student-id/Courses/course-id/Lessons/lesson-id")]
    public async Task<IActionResult> GetLessonById(GetLessonByIdRequestModel model)
    {
        var lesson = await _studentService.GetLessonByIdAsync(model);

        if (_studentService.IsValid)
            return Ok(lesson);

        _studentService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }




    [HttpGet("/api/v1/Students/active-count")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> GetStudentsCount()
    {
        int count = await _studentService.GetActiveStudentCountAsync();


        if (_studentService.IsValid)
            return Ok(count);

        _studentService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpPost("/api/v1/Courses/course-id/students-count")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> GetStudentsCountByCourseId(GetStudentsByCourseIdModel model)
    {
        int count = await _studentService.GetTotalStudentsCountByCourseId(model);

        if (_studentService.IsValid)
            return Ok(count);

        _studentService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


    [HttpPost("/api/v1/Instructors/intructor-id/students-count")]
    [Authorize(Roles ="Admin, Instructor")]
    public async Task<IActionResult> GetStudentsCountByInstructorId(GetInstructorStudentsCount model)
    {
        int count = await _studentService.GetStudentsCountByInstructorIdAsync(model);

        if (_studentService.IsValid)
            return Ok(count);

        _studentService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Instructor")]
    public async Task<IActionResult> Create(CreateStudentModel model)
    {
        await _studentService.CreateAsync(model);

        if (_studentService.IsValid)
            return Ok("Done");

        _studentService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }
}