using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace Core.Plugins.Samples.Domain.Entities
{
    public partial class OrderEntity : Core.Framework.IAuditable
    {
        public long OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public byte[] RowVersion { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
