namespace OnlineCourse.Application.Dtos;

public class PaymentDto
{
    public Guid UserID { get; set; }
    public decimal Amount { get; set; }
    public int CourseId { get; set; }
    public bool IsVerified { get; set; } = false;
    public DateTime PaymentDate { get; set; }
}
