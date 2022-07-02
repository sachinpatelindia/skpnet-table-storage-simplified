using System;

namespace SKP.Net.Core.Domain.Messages
{
    public partial class EmailAccount:BaseEntity
    {
        public EmailAccount()
        {
            PartitionKey = nameof(EmailAccount);
            RowKey = Guid.NewGuid().ToString();
        }
        public string From { get; set; }
        public string DisplayName { get; set; }
        public string SmtpUserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
    }
}
