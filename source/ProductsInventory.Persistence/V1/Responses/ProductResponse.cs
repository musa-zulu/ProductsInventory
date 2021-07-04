using System;

namespace ProductsInventory.Persistence.V1.Responses
{
    public class ProductResponse
    {
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }

        public Guid CategoryId { get; set; }      
        public Guid UserId { get; set; }
    }
}
