using AutoMapper;
using KA.DataProvider;
using KA.DataProvider.Entities;
using KA.Infrastructure.Enums;
using KA.Repository.Base;
using KA.Service.Carts;
using KA.Service.Mapper;
using KA.ViewModels.Carts;
using Moq;
using System;
using Xunit;

namespace KA.UnitTests
{
    public class CartServiceTests
    {
        [Fact]
        public async Task Add_WithNewCart_ReturnCart()
        {
            // Arrange
            var dbContextFactory = new Mock<ApplicationDbContextFactory>();
            var dbContext = dbContextFactory.Object.CreateDbContext(null);

            var cartRepository = new BaseRepository<Cart>(dbContext);
            var userRepository = new BaseRepository<AppUser>(dbContext);

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = mockMapper.CreateMapper();
           
            var cartService = new CartService(cartRepository,mapper);
            var userUd = (await userRepository.GetFirstOrDefaultAsync(u=>u.UserName =="vanson")).Id;
            // Act
            var reuslt = await cartService.CreateNewCart(new CreateCartVm()
            {
                CartStatus = CartStatus.PreOrder,
                UserId = userUd
            });
            // Assert
            Assert.True(reuslt!= null);
        }
    }
}