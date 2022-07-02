using System;

namespace SKP.Net.Core.Domain.Security
{
    public class PermissionRecord : BaseEntity
    {
        public PermissionRecord()
        {
            PartitionKey = nameof(PermissionRecord);
            RowKey = Guid.NewGuid().ToString();
        }

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
