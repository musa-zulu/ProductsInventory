using PeanutButter.RandomGenerators;
using ProductsInventory.DB.Domain;
using System;

namespace ProductInventory.Tests.Common.Builders.Domain
{
    public class CategoryBuilder : GenericBuilder<CategoryBuilder, Category>
    {
        public CategoryBuilder WithId(Guid id)
        {
            return WithProp(x => x.CategoryId = id);
        }
    }
}