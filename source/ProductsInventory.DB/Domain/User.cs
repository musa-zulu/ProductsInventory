using System;

namespace ProductsInventory.DB.Domain
{
    public class User : EntityBase
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public bool IsLocked { get; set; }        
    }
}
