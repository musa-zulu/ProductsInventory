﻿using ProductsInventory.DB.Domain;
using System;

namespace ProductsInventory.Persistence.V1.Requests
{
    public class UpdateCategoryRequest : EntityBase
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string CategoryCode { get; set; }
        public bool IsActive { get; set; }
    }
}