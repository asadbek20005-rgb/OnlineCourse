using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Application.Models.Payment;

public class CreatePaymentModel
{
    [Required]
    public Guid UserID { get; set; }
    [Required]
    public decimal Amount { get; set; }

    [Required]
    public int CourseId { get; set; }
    [Required]
    public DateTime PaymentDate { get; set; }
}