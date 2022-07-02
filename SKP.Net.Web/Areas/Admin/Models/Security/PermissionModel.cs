using SKP.Net.Core.Domain.Security;

namespace SKP.Net.Web.Areas.Admin.Models.Security
{
    public class PermissionModel
    {
        public string RowKey { get; set; }
        public string Name { get; set; }
        public string SystemName { get; set; }
        public int PermissionProviderId { get; set; }
        public string? RoleRowKey { get; set; }
        public PermissionProvider PermissionProvider
        {
            get => (PermissionProvider)PermissionProviderId;
            set => PermissionProviderId = (int)value;
        }
        public bool Active { get; set; }
    }
}
