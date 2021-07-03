using System;

namespace ProductsInventory.Persistence.V1.Requests
{
    public class RequestBase
    {
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateLastModified { get; set; }
    }
}
