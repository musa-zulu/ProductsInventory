using ProductsInventory.DB.Domain;
using System;
using System.Collections.Generic;

namespace ProductsInventory.Persistence.V1.Responses
{
    public class CategoryResponse : EntityBase
    {        
        public Guid CategoryId { get; set; }     
        public string Name { get; set; }        
        public string CategoryCode { get; set; }        
        public bool IsActive { get; set; }
        public Guid UserId { get; set; }
        public virtual List<ProductResponse> Products { get; set; } //test this
    }
}
