using OnlineCourse.Application.Models.Email;

namespace OnlineCourse.Application.ServiceContracts;

public interface IEmailSenderService
{
    Task SendAsync(EmailSenderModel model);
}
