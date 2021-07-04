﻿using System;

namespace ProductsInventory.DB.Domain
{
    public class Product : EntityBase
    {
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }

        public Guid CategoryId { get; set; }
        public virtual Category CategoryName { get; set; }

    }
}
