using SKP.Net.Core.Domain.Messages;
using System;
using System.Collections.Generic;

namespace SKP.Net.Web.Areas.Admin.Models.Messages
{
    public class EmailTemplateModel
    {
        public string RowKey { get; set; }
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
