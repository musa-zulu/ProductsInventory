using ProductsInventory.DB.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsInventory.Persistence.Interfaces.Services
{
    public interface IUserService
    {        
        Task<List<User>> GetUsersAsync(PaginationFilter paginationFilter = null);
        Task<bool> CreateUserAsync(User user);
        Task<User> GetUserByUserNameAsync(string userName);
        Task<User> GetUserByUserIdAsync(Guid userId);
        Task<bool> UpdateUserAsync(User userToUpdate);
        Task<bool> DeleteUserAsync(Guid userId);
    }
}
