using System;

namespace ProductsInventory.DB.Domain
{
    public class Error : EntityBase
    {
        public Guid ErrorId { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
