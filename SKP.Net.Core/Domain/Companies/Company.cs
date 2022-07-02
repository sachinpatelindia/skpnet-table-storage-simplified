using System;

namespace SKP.Net.Core.Domain.Companies
{
    public partial class Company:BaseEntity
    {
        public Company()
        {
            PartitionKey = nameof(Company);
            RowKey = Guid.NewGuid().ToString();
        }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string Domain { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

    }
}
