using System;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using ProductInventory.Tests.Common.Builders.Domain;
using ProductInventory.Tests.Common.Helpers;
using ProductsInventory.DB;
using ProductsInventory.DB.Domain;
using ProductsInventory.Persistence.Services;

namespace ProductInventory.Tests.Persistence.Services
{
    [TestFixture]
    public class TestUserService
    {
        private FakeDbContext _db;
        [SetUp]
        public void SetUp()
        {
            _db = new FakeDbContext(Guid.NewGuid().ToString());
            _db.DbContext.Database.EnsureCreated();
        }

        [Test]
        public void Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() =>
                new UserService(Substitute.For<IApplicationDbContext>()));
            //---------------Test Result -----------------------
        }

        [Test]
        public async Task GetUsersAsync_GivenNoUserExist_ShouldReturnEmptyList()
        {
            //---------------Set up test pack-------------------            
            var userService = CreateUserService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await userService.GetUsersAsync();
            //---------------Test Result -----------------------
            Assert.IsEmpty(results);
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public async Task GetUsersAsync_GivenOneUserExist_ShouldReturnListWithThatUser()
        {
            //---------------Set up test pack-------------------
            var user = CreateRandomUser(Guid.NewGuid());
            await _db.Add(user);

            var userService = CreateUserService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await userService.GetUsersAsync();
            //---------------Test Result -----------------------
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(user.Username, results[0].Username);
            Assert.AreEqual(user.Email, results[0].Email);
        }

        [Test]
        public async Task GetUsersAsync_GivenTwoUsersExist_ShouldReturnAListWithTwoUsers()
        {
            //---------------Set up test pack-------------------
            var firstUser = CreateRandomUser(Guid.NewGuid());
            var secondUser = CreateRandomUser(Guid.NewGuid());

            await _db.Add(firstUser, secondUser);

            var userService = CreateUserService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await userService.GetUsersAsync();
            //---------------Test Result -----------------------
            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count);
        }

        [Test]
        public async Task GetUsersAsync_GivenManyUsersExist_ShouldReturnAListWithAllUsers()
        {
            //---------------Set up test pack-------------------
            var firstUser = CreateRandomUser(Guid.NewGuid());
            var secondUser = CreateRandomUser(Guid.NewGuid());
            var thirdUser = CreateRandomUser(Guid.NewGuid());
            var forthUser = CreateRandomUser(Guid.NewGuid());

            await _db.Add(firstUser, secondUser, thirdUser, forthUser);

            var userService = CreateUserService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await userService.GetUsersAsync();
            //---------------Test Result -----------------------
            Assert.IsNotNull(results);
            Assert.AreEqual(4, results.Count);
        }

        [Test]
        public async Task CreateUserAsync_GivenAUserExistOnDb_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var user = CreateRandomUser(Guid.NewGuid());
            await _db.Add(user);

            var userService = CreateUserService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await userService.CreateUserAsync(user);
            //---------------Test Result -----------------------
            Assert.IsFalse(results);
        }

        [Test]
        public async Task CreateUserAsync_GivenAUser_ShouldAddUserToRepo()
        {
            //---------------Set up test pack-------------------
            var user = CreateRandomUser(Guid.NewGuid());
            var userService = CreateUserService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await userService.CreateUserAsync(user);
            //---------------Test Result -----------------------
            var userFromRepo = await userService.GetUserByUserIdAsync(user.UserId);
            Assert.IsTrue(results);
            Assert.AreEqual(userFromRepo.UserId, user.UserId);
            Assert.AreEqual(userFromRepo.Username, user.Username);            
            Assert.AreEqual(userFromRepo.Email, user.Email);            
        }

        [Test]
        public async Task CreateUserAsync_GivenAUserHasBeenSavedSuccessfully_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var user = CreateRandomUser(Guid.NewGuid());
            var userService = CreateUserService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await userService.CreateUserAsync(user);
            //---------------Test Result -----------------------            
            Assert.IsTrue(results);          
        }

        [Test]
        public async Task GetUserByUserIdAsync_GivenNoUserExist_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------  
            var userService = CreateUserService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await userService.GetUserByUserIdAsync(Guid.NewGuid());
            //---------------Test Result -----------------------
            Assert.IsNull(results);
        }

        [Test]
        public async Task GetUserByUserIdAsync_GivenUserExistInRepo_ShouldReturnThatUser()
        {
            //---------------Set up test pack-------------------
            var user = CreateRandomUser(Guid.NewGuid());
            var userService = new UserService(_db.DbContext);
            await _db.Add(user);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await userService.GetUserByUserIdAsync(user.UserId);
            //---------------Test Result -----------------------         
            Assert.AreEqual(results.Email, user.Email);
            Assert.AreEqual(results.UserId, user.UserId);
            Assert.AreEqual(results.Username, user.Username);
        }

        [Test]
        public async Task GetUserByUserNameAsync_GivenNoUserExist_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------  
            var userService = CreateUserService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await userService.GetUserByUserNameAsync(null);
            //---------------Test Result -----------------------
            Assert.IsNull(results);
        }

        [Test]
        public async Task GetUserByUserNameAsync_GivenUserExistInRepo_ShouldReturnThatUser()
        {
            //---------------Set up test pack-------------------
            var user = CreateRandomUser(Guid.NewGuid());
            var userService = new UserService(_db.DbContext);
            await _db.Add(user);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await userService.GetUserByUserNameAsync(user.Username);
            //---------------Test Result -----------------------         
            Assert.AreEqual(results.Email, user.Email);
            Assert.AreEqual(results.UserId, user.UserId);
            Assert.AreEqual(results.Username, user.Username);
        }

        [Test]
        public async Task DeleteUserAsync_GivenNoUserExist_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------            
            var userService = CreateUserService();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var results = await userService.DeleteUserAsync(Guid.Empty);
            //---------------Test Result -----------------------            
            Assert.IsFalse(results);
        }

        [Test]
        public async Task UpdateUserAsync_GivenUserExistInRepo_ShouldUpdateThatUser()
        {
            //---------------Set up test pack-------------------
            var user = CreateRandomUser(Guid.NewGuid()); 
            await _db.Add(user);      
            var userService = new UserService(_db.DbContext);
            //---------------Assert Precondition----------------
            user.Username = "This has been updated";
            //---------------Execute Test ----------------------
            var results = await userService.UpdateUserAsync(user);
            //---------------Test Result -----------------------         
            var userFromRepo = await userService.GetUserByUserIdAsync(user.UserId);            
            Assert.AreEqual(userFromRepo.Username, "This has been updated");
        }

        [Test]
        public async Task UpdateUserAsync_GivenUserHasBeenUpdatedSuccessfully_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var user = CreateRandomUser(Guid.NewGuid());
            await _db.Add(user);
            var userService = new UserService(_db.DbContext);
            //---------------Assert Precondition----------------
            user.Username = "This has been updated";
            //---------------Execute Test ----------------------
            var results = await userService.UpdateUserAsync(user);
            //---------------Test Result -----------------------                     
            Assert.IsTrue(results);            
        }

        private UserService CreateUserService()
        {                      
            return new UserService(_db.DbContext);
        }

        private static User CreateRandomUser(Guid id)
        {
            var user = new UserBuilder().WithId(id).WithRandomProps().Build();
            return user;
        }

        public void Dispose()
        {
            _db.DbContext.Database.EnsureDeleted();
            _db.DbContext.Dispose();
        }
    }
}
