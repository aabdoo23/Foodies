using Microsoft.AspNetCore.Identity.UI.Services;

public class EmailSender : IEmailSender // fake one to solve the error
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        return Task.CompletedTask;
    }
}