namespace JobPortal_New.Interfaces.Repositories
{
    public interface IEmailRepository
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}

