using System;

namespace ProductsInventory.Persistence.V1.Requests
{
    public class CreateUserRequest : RequestBase
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public bool IsLocked { get; set; }
        public string Password { get; set; }
    }
}
