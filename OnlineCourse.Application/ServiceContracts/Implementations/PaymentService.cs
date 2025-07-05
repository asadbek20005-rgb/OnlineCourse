using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Payment;
using OnlineCourse.Application.RepositoryContracts;
using OnlineCourse.Domain.Entities;
using StatusGeneric;
using Stripe;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class PaymentService(IBaseRepositroy<Payment> paymentRepository,
    IMapper mapper,
    IBaseRepositroy<User> userRepository,
    IBaseRepositroy<Course> courseRepository,
    IValidator<CreatePaymentModel> createValidator,
    IValidator<HasPaidRequestModel> hasPaidReuqestValidator) : StatusGenericHandler, IPaymentService
{
    private readonly IBaseRepositroy<Payment> _paymentRepository = paymentRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IBaseRepositroy<User> _userRepository = userRepository;
    private readonly IBaseRepositroy<Course> _courseRepository = courseRepository;
    private readonly IValidator<CreatePaymentModel> _createValidator = createValidator;
    private readonly IValidator<HasPaidRequestModel> _hasPaidValidator = hasPaidReuqestValidator;
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
        var validatorResult = await _hasPaidValidator.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
        }

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

    public async Task<string> InitiateAsync(CreatePaymentModel model)
    {

        User? user = await _userRepository.GetByIdAsync(model.UserID);
        if (user is null)
        {
            AddError($"User with id: {model.UserID} is not found");
            return "";
        }

        Course? course = await _courseRepository.GetByIdAsync(model.CourseId);

        if (course is null)
        {
            AddError($"Course with id: {model.CourseId} is not found");
            return "";
        }




        var validatorResult = await _createValidator.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
        }
        HasPaidRequestModel hasPaidRequestModel = new HasPaidRequestModel
        {
            UserId = model.UserID,
            CourseId = model.CourseId,
        };

        bool hasPaid = await HasPaidAsync(hasPaidRequestModel);

        if (hasPaid)
        {
            AddError($"Payment with user id: {model.UserID} and course id: {model.CourseId} has paid before");
            return "";
        }

        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)model.Amount,
            Currency = "usd",
            PaymentMethodTypes = new List<string> { "card" }
        };

        var service = new PaymentIntentService();
        var paymentIntent = await service.CreateAsync(options);

        Payment newPayment = _mapper.Map<Payment>(model);
        newPayment.HasPaid = true;
        newPayment.TransactionId = paymentIntent.Id;
        newPayment.CourseId = course.Id;
        newPayment.UserID = user.Id;
        await _paymentRepository.AddAsync(newPayment);
        await _paymentRepository.SaveChangesAsync();

        return paymentIntent.ClientSecret;
    }

    public Task VerifyAsync(int paymentId)
    {
        throw new NotImplementedException();
    }
}
