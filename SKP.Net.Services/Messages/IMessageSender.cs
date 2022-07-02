using SKP.Net.Core.Domain.Messages;
using System.Threading.Tasks;

namespace SKP.Net.Services.Messages
{
    public interface IMessageSender
    {
        Task SendMesage(TemplateType templateType, string toEmail, string subject, string message);
    }
}