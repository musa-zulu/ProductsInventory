using PeanutButter.RandomGenerators;
using ProductsInventory.DB.Domain;
using System;

namespace ProductInventory.Tests.Common.Builders.Domain
{
    public class UserBuilder : GenericBuilder<UserBuilder, User>
    {
        public UserBuilder WithId(Guid id)
        {
            return WithProp(x => x.UserId = id);
        }
    }
}