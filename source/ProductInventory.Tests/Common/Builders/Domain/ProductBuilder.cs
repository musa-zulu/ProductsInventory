using PeanutButter.RandomGenerators;
using ProductsInventory.DB.Domain;
using System;

namespace ProductInventory.Tests.Common.Builders.Domain
{
    public class ProductBuilder : GenericBuilder<ProductBuilder, Product>
    {
        public ProductBuilder WithId(Guid id)
        {
            return WithProp(x => x.ProductId = id);
        }
    }
}