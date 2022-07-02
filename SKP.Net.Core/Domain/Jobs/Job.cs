namespace SKP.Net.Core.Domain.Jobs
{
    public partial class Job:BaseEntity
    {
        public string CompanyRowKey{ get; set; }
        public string PostName { get; set; }
        public string Skills { get; set; }
        public bool Active { get; set; }
    }
}
