namespace OnlineCourse.Application.Models.Payment;
public class InitiatePaymentResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? ClientSecret { get; set; }
}
