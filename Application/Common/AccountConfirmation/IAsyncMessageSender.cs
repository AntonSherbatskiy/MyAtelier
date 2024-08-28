namespace Application.Common.AccountConfirmation;

public interface IAsyncMessageSender
{
    public Task SendAsync(string email, string subject, string message);
}