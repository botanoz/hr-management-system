using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrManagementSystem.ServiceLayer.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
        Task SendEmailAsync(string to, string subject, string body, List<Attachment> attachments, bool isHtml = false);
        Task SendEmailToMultipleRecipientsAsync(List<string> to, string subject, string body, bool isHtml = false);
        Task SendEmailWithTemplateAsync(string to, string subject, string templateName, object model);
    }

    public class Attachment
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }
}