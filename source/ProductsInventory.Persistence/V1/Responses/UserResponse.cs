using System;

namespace ProductsInventory.Persistence.V1.Responses
{
    public class UserResponse
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
