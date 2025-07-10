using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Application.Extensions;
using OnlineCourse.Application.Models.Payment;
using OnlineCourse.Application.ServiceContracts;

namespace OnlineCourse.Server.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Asp.Versioning.ApiVersion("1.0")]
public class PaymentsController(IPaymentService paymentService) : ControllerBase
{
    private readonly IPaymentService _paymentService = paymentService;
    [HttpPost]
    public async Task<IActionResult> Create(CreatePaymentModel model)
    {
        var res = await _paymentService.InitiateAsync(model);
        if (_paymentService.IsValid)
            return Ok(res);

        _paymentService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory(Guid userId)
    {
        var payments = await _paymentService.GetByUser(userId);

        if (_paymentService.IsValid)
        {
            return Ok(payments);
        }

        _paymentService.CopyToModelState(ModelState);
        return BadRequest(ModelState);
    }


}
