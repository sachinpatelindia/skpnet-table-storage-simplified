using System;

namespace SKP.Net.Core.Domain.Messages
{
    public class EmailTemplate : BaseEntity
    {
        public EmailTemplate()
        {
            PartitionKey = nameof(EmailTemplate);
            RowKey = Guid.NewGuid().ToString();
        }

        public string Template { get; set; }
        public bool Active { get; set; }
        public int TemplateTypeId { get; set; }
        public TemplateType TemplateType
        {
            get => (TemplateType)TemplateTypeId;
            set => TemplateTypeId = (int)value;
        }
    }
}
