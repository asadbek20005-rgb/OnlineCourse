
using FluentEmail.Core;
using OnlineCourse.Application.Models.Email;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class EmailSenderService(IFluentEmail fluentEmail) : IEmailSenderService
{
    private readonly IFluentEmail _fluentEmail = fluentEmail;

    public async Task SendAsync(EmailSenderModel model)
    {

        await _fluentEmail
             .To(model.To)
             .Subject(model.Subject)
             .Body(model.Body, isHtml: true)
             .SendAsync();
    }
}
