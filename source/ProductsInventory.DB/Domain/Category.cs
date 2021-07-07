using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductsInventory.DB.Domain
{
    public class Category : EntityBase
    {
        [Key]
        public Guid CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string CategoryCode { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public Guid UserId { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
