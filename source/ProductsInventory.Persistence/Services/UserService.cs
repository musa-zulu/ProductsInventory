using Microsoft.EntityFrameworkCore;
using ProductsInventory.DB;
using ProductsInventory.DB.Domain;
using ProductsInventory.Persistence.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsInventory.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationDbContext _dataContext;

        public UserService(IApplicationDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            var existingUser = await _dataContext.Users
                                                .AsNoTracking()
                                                .SingleOrDefaultAsync(x => x.Username == user.Username);
            if (existingUser != null)
                return false;

            _dataContext.Users.Add(user);
            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await GetUserByUserIdAsync(userId);
            if (user == null)
                return false;

            _dataContext.Users.Remove(user);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<User> GetUserByUserIdAsync(Guid userId)
        {
            return await _dataContext.Users
                                     .AsNoTracking()
                                     .SingleOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _dataContext.Users
                                     .AsNoTracking()
                                     .SingleOrDefaultAsync(x => x.Username == userName);
        }

        public async Task<List<User>> GetUsersAsync(PaginationFilter paginationFilter = null)
        {
            var queryable = _dataContext.Users.AsQueryable();

            if (paginationFilter == null)
            {
                return await queryable.ToListAsync();
            }

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable
                .Skip(skip).Take(paginationFilter.PageSize).ToListAsync();
        }

        public async Task<bool> UpdateUserAsync(User userToUpdate)
        {
            _dataContext.Users.Update(userToUpdate);
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
