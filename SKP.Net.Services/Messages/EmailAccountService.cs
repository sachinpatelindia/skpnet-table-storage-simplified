using SKP.Net.Core.Domain.Messages;
using SKP.Net.Storage.Operations;
using System;
using System.Linq;

namespace SKP.Net.Services.Messages
{
    public class EmailAccountService : IEmailAccountService
    {
        private readonly ITableStorage<EmailAccount> _emailStorage;
        public EmailAccountService(ITableStorage<EmailAccount> emailStorage)
        {
            _emailStorage = emailStorage;
        }

        public void Insert(EmailAccount email)
        {
            if (email == null)
                throw new ArgumentNullException(nameof(email));
            try
            {
                _emailStorage.Insert(email);
            }
            catch (Exception ex)
            {

            }
        }

        public void Update(EmailAccount email)
        {
            if (email == null)
                throw new ArgumentNullException(nameof(email));
            try
            {
                _emailStorage.Update(email);
            }
            catch (Exception ex)
            {

            }
        }

        public void Delete(EmailAccount email)
        {
            if (email == null)
                throw new ArgumentNullException(nameof(email));
            try
            {
                _emailStorage.Delete(email);
            }
            catch (Exception ex)
            {

            }
        }

        public EmailAccount GetDefaultEmailAccount()
        {
            return _emailStorage.GetAll<EmailAccount>().FirstOrDefault();
        }
    }
}
