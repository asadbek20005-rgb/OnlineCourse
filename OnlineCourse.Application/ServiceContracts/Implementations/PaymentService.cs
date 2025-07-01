using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Payment;
using OnlineCourse.Application.RepositoryContracts;
using OnlineCourse.Domain.Entities;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class PaymentService(IBaseRepositroy<Payment> paymentRepository,
    IMapper mapper,
    IBaseRepositroy<User> userRepository,
    IBaseRepositroy<Course> courseRepository) : StatusGenericHandler, IPaymentService
{
    private readonly IBaseRepositroy<Payment> _paymentRepository = paymentRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IBaseRepositroy<User> _userRepository = userRepository;
    private readonly IBaseRepositroy<Course> _courseRepository = courseRepository;
    public async Task<PaymentDto?> GetByIdAsync(int paymentId)
    {
        Payment? payment = await _paymentRepository.GetByIdAsync(paymentId);
        if (payment is null)
        {
            AddError($"Payment with id: {paymentId} is not found");
            return null;
        }

        return _mapper.Map<PaymentDto>(payment);
    }

    public async Task<IEnumerable<PaymentDto>> GetByUser(Guid userId)
    {

        User? user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            AddError($"User with id: {userId} is not found");
            return Enumerable.Empty<PaymentDto>();
        }

        var userPayments = await _paymentRepository.GetAll()
            .Where(x => x.UserID == user.Id)
            .ToListAsync();

        return _mapper.Map<List<PaymentDto>>(userPayments);
    }

    public async Task<decimal> GetTotalPaidAsync(Guid userId)
    {
        decimal totalPaid = 0;

        User? user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            AddError($"User with id: {userId} is not found");
            return 0;
        }

        var userPayments = await _paymentRepository.GetAll()
           .Where(x => x.UserID == user.Id)
           .Select(x => x.Amount)
           .ToListAsync();

        totalPaid += userPayments.Sum();

        return totalPaid;
    }

    public async Task<bool> HasPaidAsync(HasPaidRequestModel model)
    {

        User? user = await _userRepository.GetByIdAsync(model.UserId);
        if (user is null)
        {
            AddError($"User with id: {model.UserId} is not found");
            return false;
        }

        Course? course = await _courseRepository.GetByIdAsync(model.CourseId);

        if (course is null)
        {
            AddError($"Course with id: {model.CourseId} is not found");
            return false;
        }

        var payment = await _paymentRepository.GetAll()
           .Where(x => x.UserID == user.Id && x.CourseId == model.CourseId)
           .FirstOrDefaultAsync();


        if (payment is null)
        {
            AddError($"Payment with user id: {model.UserId} and course id: {model.CourseId} is not found");
            return false;
        }
        return payment.HasPaid;


    }

    public async Task InitiateAsync(CreatePaymentModel model)
    {
        HasPaidRequestModel hasPaidRequestModel = new HasPaidRequestModel
        {
            UserId = model.UserID,
            CourseId = model.CourseId,
        };

        bool hasPaid = await HasPaidAsync(hasPaidRequestModel);

        if (hasPaid)
        {
            AddError($"Payment with user id: {model.UserID} and course id: {model.CourseId} has paid before");
            return;
        }

        Payment newPayment = _mapper.Map<Payment>(model);
        newPayment.HasPaid = true;
        await _paymentRepository.AddAsync(newPayment);
        await _paymentRepository.SaveChangesAsync();
    }

    public Task VerifyAsync(int paymentId)
    {
        throw new NotImplementedException();
    }
}
