using ProjectHK3.DTOs;

namespace ProjectHK3.Services.EmaiServices;

public interface IEmailService
{
    void SendEmail(EmailDto request);
}
