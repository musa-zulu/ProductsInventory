using System;

namespace ProductsInventory.Persistence.Helpers
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
        DateTime Today { get; }
    }
}
