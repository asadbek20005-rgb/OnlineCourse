namespace OnlineCourse.Application.Models.User;

public class ChangeRoleModel
{
    public Role Role { get; set; }
}


public enum Role
{
    Student, 
    Instructor
}
