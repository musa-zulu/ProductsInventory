using System;

namespace ProductsInventory.DB.Domain
{
    public class EntityBase
    {
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateLastModified { get; set; }
    }
}
