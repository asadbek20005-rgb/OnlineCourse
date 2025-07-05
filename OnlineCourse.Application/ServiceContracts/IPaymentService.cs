using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Payment;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts;

public interface IPaymentService : IStatusGeneric
{
    Task<PaymentDto?> GetByIdAsync(int paymentId);
    Task<IEnumerable<PaymentDto>> GetByUser(Guid userId);
    Task<string> InitiateAsync(CreatePaymentModel model);
    Task VerifyAsync(int paymentId);
    Task<bool> HasPaidAsync(HasPaidRequestModel model);
    Task<decimal> GetTotalPaidAsync(Guid userId);
}
