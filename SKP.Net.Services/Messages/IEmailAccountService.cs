using SKP.Net.Core.Domain.Messages;

namespace SKP.Net.Services.Messages
{
    public interface IEmailAccountService
    {
        void Delete(EmailAccount email);
        EmailAccount GetDefaultEmailAccount();
        void Insert(EmailAccount email);
        void Update(EmailAccount email);
    }
}