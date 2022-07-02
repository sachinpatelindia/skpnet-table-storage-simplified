using System;

namespace SKP.Net.Core.Domain.Media
{
    public class Image : BaseEntity
    {
        public Image()
        {

            PartitionKey = nameof(Image);
            RowKey = Guid.NewGuid().ToString();
        }
        public string ParentRowKey { get; set; }
        public string FileName { get; set; }
        public string ImageUrl { get; set; }
        public bool Active { get; set; }
        public int ImageTypeId { get; set; }
        public long Size { get; set; }
        public ImageType ImageType
        {
            get => (ImageType)ImageTypeId;
            set => ImageTypeId = (int)value;
        }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }
}
