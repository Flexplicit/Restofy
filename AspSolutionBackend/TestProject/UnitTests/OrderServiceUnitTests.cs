using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.DTO.MappingProfiles;
using BLL.App.Services;
using DAL.APP.EF;
using DAL.APP.EF.Mappers;
using DAL.APP.EF.Repositories;
using Domain.Identity;
using Domain.OrderModels;
using Domain.OrderModels.DbEnums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;
using Order = DAL.App.DTO.OrderModels.Order;

namespace TestProject.UnitTests
{
    public class OrderServiceUnitTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly AppDbContext _ctx;
        private readonly OrderService _service;
        private readonly IMapper Mapper;


        public OrderServiceUnitTests(ITestOutputHelper testOutputHelper)
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DAL.App.DTO.MappingProfiles.AutoMapperProfile>();
                cfg.AddProfile<BLL.App.DTO.MappingProfiles.AutoMapperProfile>();
            });
            var mapper = mockMapper.CreateMapper();
            Mapper = mapper;
            _testOutputHelper = testOutputHelper;

            var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            _ctx = new AppDbContext(optionBuilder.Options);
            _ctx.Database.EnsureDeleted();
            _ctx.Database.EnsureDeleted();

            var uow = new AppUnitOfWork(_ctx, mapper);
            var orderRepository = new OrderRepository(_ctx, mapper);
            _service = new OrderService(uow, orderRepository, mapper);
        }

        [Fact]
        public async Task Action_Order_MakeAnOrder()
        {
            await SeedMainFlowData();
            var foodInOrderObject = await _ctx.FoodInOrders
                .Select(x => x.Id)
                .ToListAsync();
            foodInOrderObject.Should().NotBeNull();
            foodInOrderObject.Count.Should().Be(2);
            var appUser = await _ctx.Users
                .FirstAsync();
            appUser.Should().NotBeNull();
            var paymentType = await _ctx.PaymentTypes.FirstAsync();
            paymentType.Should().NotBeNull();
            _ctx.ChangeTracker.Clear();
            var serviceResult =
                await _service.MakeAnOrderAsync(foodInOrderObject, appUser.Id, paymentType.Id, null);
            //Tests main function flow
            serviceResult.Should().NotBeNull();
            serviceResult!.Id.Should().NotBeEmpty();
            serviceResult.AppUserId.Should().NotBeEmpty();
            serviceResult.AppUserId.Should().Be(appUser.Id);
            serviceResult.OrderNumber.Should().NotBe(0);
            serviceResult.OrderCompletionStatus.Should().Be(EOrderStatus.NotConfirmed);
            serviceResult.RestaurantId.Should().NotBeEmpty();
            //Test Side effects of function
            var modifiedFoodInOrder = await _ctx.FoodInOrders
                .Where(x => foodInOrderObject.Contains(x.Id) && x.OrderId.Equals(serviceResult.Id))
                .ToListAsync();
            modifiedFoodInOrder.Count.Should().Be(2);
        }

        [Fact]
        public async Task Action_Order_ConfirmOrderByRestaurantAsyncSuccess()
        {
            await Action_Order_MakeAnOrder();
            var order = await _ctx.Orders
                .FirstOrDefaultAsync();
            order.Should().NotBeNull();
            var restaurantOwner = _ctx.Restaurants
                .Include(x => x.AppUser)
                .FirstOrDefault();
            restaurantOwner.Should().NotBeNull();
            restaurantOwner!.AppUserId.Should().NotBeEmpty();
            _ctx.ChangeTracker.Clear();
            var confirmation = await _service.ConfirmOrderByRestaurantAsync(order.Id, 30, restaurantOwner.AppUserId);

            confirmation.Should().NotBeNull();
            confirmation!.ReadyAt.Should().NotBeNull();
            confirmation.IsConfirmedByRestaurant.Should().BeTrue();
            confirmation.IsConfirmedByAppUser.Should().BeTrue();
            confirmation.OrderCompletionStatus.Should().Be(EOrderStatus.Cooking);
        }

        [Fact]
        public async Task Action_Order_GetAllUserOrdersSuccess()
        {
            await SeedRandomOrders(new Random().Next(5, 50));
            var users = await _ctx.Users
                .Include(x => x.Orders)
                .Where(x => x.Orders!.Count > 0)
                .ToListAsync();
            users.Should().NotBeNull().And.NotBeEmpty();

            foreach (var user in users)
            {
                var orders = (await _service.GetAllUserOrdersAsync(user.Id)).ToList();
                orders.Should().NotBeNull();
                orders.Should().NotBeEmpty();
                foreach (var order in orders)
                {
                    // _testOutputHelper.WriteLine(order.Id.ToString());
                    order.AppUserId.Should().Be(user.Id);
                }
            }
        }

        [Fact]
        public async Task Action_Order_GetAllRestaurantOrders()
        {
            await SeedRandomOrders(new Random().Next(5, 50));
            var users = await _ctx.Users
                .Include(x => x.Orders)
                .Where(x => x.Orders!.Count > 0)
                .ToListAsync();
            users.Should().NotBeNull().And.NotBeEmpty();

            foreach (var user in users)
            {
                var orders = (await _service.GetAllRestaurantOrdersAsync(user.Id)).ToList();
                orders.Should().NotBeNull();

                foreach (var order in orders)
                {
                    var testableRestaurantOrder = await _ctx.Restaurants
                        .Where(x => x.Id.Equals(order.RestaurantId))
                        .FirstOrDefaultAsync();
                    testableRestaurantOrder.Should().NotBeNull();
                    // Testing if restaurant owner is the one who receives the order and not other random people
                    _testOutputHelper.WriteLine(testableRestaurantOrder.AppUserId.ToString());
                    testableRestaurantOrder.AppUserId.Should().Be(user.Id);
                }
            }
        }

        [Fact]
        public async Task Action_Order_GetAllActiveOrders()
        {
            await SeedRandomOrders(new Random().Next(5, 50));
            var users = await _ctx.Users
                .Include(x => x.Orders)
                .Where(x => x.Orders!.Count > 0)
                .ToListAsync();
            users.Should().NotBeNull().And.NotBeEmpty();

            foreach (var user in users)
            {
                var orders = (await _service.GetAllActiveOrdersAsync(user.Id)).ToList();
                orders.Should().NotBeNull();

                foreach (var order in orders)
                {
                    _testOutputHelper.WriteLine(order.OrderCompletionStatus.ToString());
                    order.OrderCompletionStatus
                        .Should().NotBe(BLL.App.DTO.OrderModels.DbEnums.EOrderStatus.Finished);
                }
            }
        }

        [Fact]
        public async Task Action_Order_CRUD_Functionalities()
        {
            await SeedMainFlowData();
            var restaurant = await _ctx.Restaurants.FirstOrDefaultAsync();
            var orderNumber = new Random().Next(1000, 50000);
            var users = await _ctx.Users.FirstOrDefaultAsync();
            //Create
            var orderCreate = _service.Add(new BLL.App.DTO.OrderModels.Order()
            {
                AppUserId = users.Id,
                CreatedAt = DateTime.Now,
                OrderCompletionStatus = BLL.App.DTO.OrderModels.DbEnums.EOrderStatus.NotConfirmed,
                OrderNumber = orderNumber,
                OrderTotalCost = 1000,
                RestaurantId = restaurant.Id,
            });
            await _ctx.SaveChangesAsync();
            _ctx.ChangeTracker.Clear();
            var order = await _ctx.Orders.Where(x => x.Id == orderCreate.Id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            order.Should().NotBeNull();
            order.AppUserId.Should().Be(users.Id);
            order.RestaurantId.Should().Be(restaurant.Id);
            orderCreate.Id.Should().Be(order.Id);
            // Read
            var orderRead = await _service.FirstOrDefaultAsync(order.Id, users.Id);
            orderRead.Should().NotBeNull();
            orderRead!.AppUserId.Should().Be(users.Id);
            orderRead!.RestaurantId.Should().Be(restaurant.Id);
            orderRead.Id.Should().Be(order.Id);
            // Update
            orderRead.OrderCompletionStatus = BLL.App.DTO.OrderModels.DbEnums.EOrderStatus.Cooking;
            var orderFirst = _service.Update(orderRead);
            await _ctx.SaveChangesAsync();
            var orderEdit = await _ctx.Orders.Where(x => x.Id == orderFirst.Id).FirstOrDefaultAsync();
            orderEdit.Should().NotBeNull();
            orderEdit!.AppUserId.Should().Be(users.Id);
            orderEdit!.RestaurantId.Should().Be(restaurant.Id);
            orderEdit.Id.Should().Be(order.Id);
            orderEdit.OrderCompletionStatus.Should().Be(EOrderStatus.Cooking);
            //Delete
            _ctx.ChangeTracker.Clear();
            var orderDelete = await _service.FirstOrDefaultAsync(orderEdit.Id, users.Id);
            _service.Remove(orderDelete, users.Id);
            await _ctx.SaveChangesAsync();
            var orderDeleteResult = await _service.FirstOrDefaultAsync(orderEdit.Id, users.Id);
            orderDeleteResult.Should().BeNull();
        }

        [Fact]
        public async Task Action_Order_Extra_Functionalities()
        {
            var amount = new Random().Next(5, 50);
            await SeedRandomOrders(amount);

            var users = await _ctx.Users.ToListAsync();
            var allOrders = new List<BLL.App.DTO.OrderModels.Order>();
            //Get All
            foreach (var user in users)
            {
                var orders = await _service.GetAllAsync(user.Id);
                allOrders.AddRange(orders);
            }

            allOrders.Count.Should().Be(amount);
            // ExistsAsync and RemoveAsync
            (await _service.ExistsAsync(allOrders[0].Id, allOrders[0].AppUserId)).Should().BeTrue();
            _ctx.ChangeTracker.Clear();
            await _service.RemoveAsync(allOrders[0].Id, allOrders[0].AppUserId);
            await _ctx.SaveChangesAsync();
            Func<Task> act = async () => { await _service.RemoveAsync(allOrders[0].Id, allOrders[0].AppUserId); };
            await act.Should().ThrowAsync<NullReferenceException>();
            (await _service.ExistsAsync(allOrders[0].Id, allOrders[0].AppUserId)).Should().BeFalse();
        }

        private async Task SeedRandomOrders(int amount)
        {
            await SeedMainFlowData();
            var restaurant = await _ctx.Restaurants.FirstOrDefaultAsync();
            var foodInOrder = await _ctx.FoodInOrders.ToListAsync();
            var users = await _ctx.Users.ToListAsync();
            var paymentType = await _ctx.PaymentTypes.FirstOrDefaultAsync();
            restaurant.Should().NotBeNull();
            foodInOrder.Should().NotBeNull().And.NotBeEmpty();
            paymentType.Should().NotBeNull();
            users.Should().NotBeNull().And.HaveCountGreaterThan(1);
            var orderStatusEnums = Enum.GetValues(typeof(EOrderStatus)).Cast<EOrderStatus>().ToList();

            // var amount = new Random().Next(5, 50);
            for (var i = 0; i < amount; i++)
            {
                var randomStatus = new Random().Next(0, orderStatusEnums.Count);
                _ctx.Orders.Add(new Domain.OrderModels.Order()
                {
                    OrderCompletionStatus = (EOrderStatus) randomStatus,
                    OrderNumber = new Random().Next(1000, 50000),
                    Restaurant = restaurant,
                    FoodInOrders = foodInOrder,
                    AppUser = users[new Random().Next(0, users.Count)],
                    CreatedAt = DateTime.Now,
                    PaymentType = paymentType
                });
            }

            await _ctx.SaveChangesAsync();
        }


        private async Task SeedMainFlowData()
        {
            // _ctx.Users.Add(new AppUser
            // {
            //     FirstName = "normalUserFirst",
            //     LastName = "normalUserLast",
            //     Email = "test@gmail.com",
            //     PasswordHash = "MostSecurePasswordEver"
            // });
            // var restaurantOwner = new AppUser
            // {
            //     FirstName = "RestaurantOwner",
            //     LastName = "RestaurantOwner",
            //     Email = "RestaurantOwner@gmail.com",
            //     PasswordHash = "MostSecurePasswordEver"
            // };
            //
            // var restaurantFood = new List<Food>()
            // {
            //     new()
            //     {
            //         FoodName = "someRandomBurger0",
            //         Picture = "someJpg",
            //         FoodGroup = new FoodGroup()
            //         {
            //             FoodGroupType = FoodGroupType.Burger
            //         },
            //         Cost = new Cost()
            //         {
            //             CostWithoutVat = 10,
            //             CostWithVat = 20,
            //             Vat = 20
            //         }
            //     },
            //     new()
            //     {
            //         FoodName = "someRandomBurger1",
            //         Picture = "jpgSome",
            //         FoodGroup = new FoodGroup()
            //         {
            //             FoodGroupType = FoodGroupType.Sushi
            //         },
            //         Cost = new Cost()
            //         {
            //             CostWithoutVat = 20,
            //             CostWithVat = 40,
            //             Vat = 20
            //         }
            //     }
            // };
            // var restaurant = new Restaurant
            // {
            //     Name = "TestRestaurant0",
            //     Description = "its description",
            //     RestaurantAddress = "TestAddress",
            //     RestaurantFood = restaurantFood,
            //     AppUser = restaurantOwner
            // };
            // _ctx.Restaurants.Add(restaurant);
            // _ctx.FoodInOrders.Add(new FoodInOrder()
            // {
            //     Id = Guid.Parse("994d4378-0eca-4de8-915d-22832057a5b1"),
            //     Amount = 2,
            //     Food = restaurantFood[0],
            // });
            // _ctx.FoodInOrders.Add(new FoodInOrder()
            // {
            //     Id = Guid.Parse("8cc328cb-aa88-4204-a779-8ba7ddd651ba"),
            //     Amount = 3,
            //     Food = restaurantFood[1]
            // });
            // _ctx.PaymentTypes.Add(new PaymentType()
            // {
            //     Type = EPaymentType.Cash
            // });
            // await _ctx.SaveChangesAsync();
        }
    }
}