using ProductsInventory.DB.Domain;
using System;

namespace ProductsInventory.Persistence.V1.Requests
{
    public class UpdateProductRequest : EntityBase
    {
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }

        public Guid CategoryId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }

        public virtual UpdateCategoryRequest Category { get; set; }
    }
}
