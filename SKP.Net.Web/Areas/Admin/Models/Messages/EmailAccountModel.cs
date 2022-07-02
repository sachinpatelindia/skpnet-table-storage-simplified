namespace SKP.Net.Web.Areas.Admin.Models.Messages
{
    public class EmailAccountModel
    {
        public string RowKey { get; set; }
        public string From { get; set; }
        public string DisplayName { get; set; }
        public string SmtpUserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
    }
}
