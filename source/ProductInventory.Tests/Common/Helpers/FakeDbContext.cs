using Microsoft.EntityFrameworkCore;
using ProductsInventory.DB;
using System.Threading.Tasks;

namespace ProductInventory.Tests.Common.Helpers
{
    public class FakeDbContext
    {
        public FakeDbContext(string name)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(name)
                         .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                         .Options;

            DbContext = new ApplicationDbContext(options);
        }
        public ApplicationDbContext DbContext;

        public async Task Add(params object[] data)
        {
            DbContext.AddRange(data);
            await DbContext.SaveChangesAsync();
        }
    }
}
