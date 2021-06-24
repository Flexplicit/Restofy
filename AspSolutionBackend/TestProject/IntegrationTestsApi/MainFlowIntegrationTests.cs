using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PublicApiDTO.v1.v1.AuthenticationDTO;
using PublicApiDTO.v1.v1.OrderModels;
using PublicApiDTO.v1.v1.OrderModels.DbEnums;
using TestProject.Helpers;
using Xunit;
using Xunit.Abstractions;
using Food = PublicApiDTO.v1.v1.OrderModels.Food;
using FoodGroup = PublicApiDTO.v1.v1.OrderModels.FoodGroup;
using FoodInOrder = PublicApiDTO.v1.v1.OrderModels.FoodInOrder;
using Order = PublicApiDTO.v1.v1.OrderModels.Order;
using PaymentType = PublicApiDTO.v1.v1.OrderModels.PaymentType;
using Restaurant = PublicApiDTO.v1.v1.OrderModels.Restaurant;

namespace TestProject.IntegrationTestsApi
{
    public class MainFlowIntegrationTests : IClassFixture<CustomWebApplicationFactory<WebApp.Startup>>
    {
        private readonly CustomWebApplicationFactory<WebApp.Startup> _factory;
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _testOutputHelper;

        public MainFlowIntegrationTests(CustomWebApplicationFactory<WebApp.Startup> factory,
            ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions()
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        //https://stackoverflow.com/questions/23585919/send-json-via-post-in-c-sharp-and-receive-the-json-returned
        public async Task Test_Api_Order_MainFlow()
        {
            var jwtNormalUser = "";
            var jwtRestaurantOwner = "";
            var uri = "/api/v1/Account/Register";
            var httpContentNormalUser = GenerateHttpJsonContent(new
            {
                Email = "normalUser@gmail.com",
                Password = "SecurePassword9$",
                Firstname = "firstName",
                Lastname = "lastName"
            });
            var httpContentRestaurantUser = GenerateHttpJsonContent(new
            {
                Email = "restaurantOwner@gmail.com",
                Password = "NotSoSecurePassword1#",
                Firstname = "Big",
                Lastname = "Gus"
            });
            var registerNormalUserResponse = await _client.PostAsync(uri, httpContentNormalUser);
            var registerRestaurantUserResponse = await _client.PostAsync(uri, httpContentRestaurantUser);
            registerNormalUserResponse.EnsureSuccessStatusCode();
            registerRestaurantUserResponse.EnsureSuccessStatusCode();

            var dataNormalUser = JsonHelper.DeserializeWithWebDefaults<JwtLogin>
                (await registerNormalUserResponse.Content.ReadAsStringAsync());
            var dataRestaurantUser = JsonHelper.DeserializeWithWebDefaults<JwtLogin>
                (await registerRestaurantUserResponse.Content.ReadAsStringAsync());
            dataNormalUser.Should().NotBeNull();
            dataRestaurantUser.Should().NotBeNull();
            jwtNormalUser = dataNormalUser!.Token;
            jwtRestaurantOwner = dataRestaurantUser!.Token;

            //Adding a restaurant and food to one of the users(RestaurantOwner)
            uri = "api/v1/Restaurants/Create";
            var httpContentUserRestaurantCreate = GenerateHttpJsonContent(new
            {
                Name = "Kfc",
                RestaurantAddress = "Kase 11",
                Description = "Juicy Chicken Wings",
                Picture = "https://i.gyazo.com/facdeb6c28ae02b891e0e56fe92d60d1.png"
            });
            uri = "/api/v1/Restaurant";
            AddAuthentication(jwtRestaurantOwner, _client);
            var createUserRestaurant = await _client.PostAsync(uri, httpContentUserRestaurantCreate);
            createUserRestaurant.EnsureSuccessStatusCode();
            var restaurant = JsonHelper.DeserializeWithWebDefaults<Restaurant>
                (await createUserRestaurant.Content.ReadAsStringAsync());
            restaurant.Should().NotBeNull().And.BeOfType<Restaurant>();
            restaurant!.Id.Should().NotBeEmpty();

            // Getting foodgroups for out food that we add

            var foodGroups = await GetSingularEntity<FoodGroup[]>("/api/v1/FoodGroup/GetFoodGroups");
            foodGroups.Should().NotBeNull().And.NotBeEmpty().And.HaveCountGreaterOrEqualTo(1);
            var restaurantFoodHttpContent = new FoodCreate
            {
                CostWithVat = 10,
                Description = "Juicy Chicken Wing",
                FoodName = "BBQ Wing",
                Picture = "https://i.gyazo.com/4a83e0ad21bfb60ce20d6375a4c0f0a9.png",
                FoodGroupId = foodGroups![0].Id,
                RestaurantId = restaurant.Id
            };
            uri = "/api/v1/Food/Create";
            var createRestaurantFood = await _client.PostAsync(uri, GenerateHttpJsonContent(restaurantFoodHttpContent));
            createRestaurantFood.EnsureSuccessStatusCode();

            //Check if it got added
            uri = "api/v1/Food/GetFoods/" + restaurant.Id;
            var restaurantFoods = await _client.GetAsync(uri);
            restaurantFoods.EnsureSuccessStatusCode();
            var foods = JsonHelper.DeserializeWithWebDefaults<Food[]>
                (await restaurantFoods.Content.ReadAsStringAsync());
            foods.Should().NotBeNull().And.HaveCount(1);
            foods![0].RestaurantId.Should().Be(restaurant.Id);

            //Normal user orders food
            uri = "api/v1/FoodInOrder/";
            var foodOrder = new List<FoodInOrder>()
            {
                new()
                {
                    Amount = 3,
                    FoodId = foods[0].Id
                }
            };
            AddAuthentication(jwtNormalUser, _client);
            var foodInOrderRequest = await _client.PostAsync(uri, GenerateHttpJsonContent(foodOrder));
            var foodInOrderResp = JsonHelper.DeserializeWithWebDefaults<FoodInOrder[]>
                (await foodInOrderRequest.Content.ReadAsStringAsync());
            foodInOrderRequest.EnsureSuccessStatusCode();
            foodInOrderResp.Should().NotBeNull().And.NotBeEmpty();
            uri = "api/v1/Order/CreateOrder";
            var paymentTypes = await GetAllPaymentTypes();
            var orderCreate = new OrderCreate()
            {
                FoodInOrderId = foodInOrderResp!.Select(x => x.Id),
                PaymentTypeId = paymentTypes.First().Id
            };
            var orderRequest = await _client.PostAsync(uri, GenerateHttpJsonContent(orderCreate));
            orderRequest.EnsureSuccessStatusCode();
            var orderResponse = JsonHelper.DeserializeWithWebDefaults<Order>
                (await orderRequest.Content.ReadAsStringAsync())!;
            orderResponse.Should().NotBeNull();
            orderResponse.IsConfirmedByRestaurant.Should().BeFalse();
            orderResponse.IsConfirmedByAppUser.Should().BeFalse();
            orderResponse.OrderCompletionStatus.Should().Be(EOrderStatus.NotConfirmed);
            //Confirming order
            uri = "api/v1/Order/OrderConfirm";
            var orderConfirmation = new OrderConfirmRestaurant()
            {
                MinutesTillReady = 50,
                OrderId = orderResponse.Id
            };
            AddAuthentication(jwtRestaurantOwner, _client);
            var orderConfirmRequest = await _client.PostAsync(uri, GenerateHttpJsonContent(orderConfirmation));
            orderConfirmRequest.EnsureSuccessStatusCode();
            var orderConfirmResponse = JsonHelper.DeserializeWithWebDefaults<Order>
                (await orderConfirmRequest.Content.ReadAsStringAsync())!;
            orderConfirmResponse.Id.Should().Be(orderConfirmation.OrderId);
            // orderConfirmResponse.ReadyAt.Should().Be(DateTime.Now.AddMinutes(50));
            orderConfirmResponse.IsConfirmedByRestaurant.Should().BeTrue();
            orderConfirmResponse.IsConfirmedByAppUser.Should().BeTrue();
            orderConfirmResponse.OrderCompletionStatus.Should().Be(EOrderStatus.Cooking);

            // Changing order status to ready and then finishing order
            orderConfirmResponse.OrderCompletionStatus = EOrderStatus.Ready;
            await EditEntity(orderConfirmResponse, "api/v1/Order/PutOrder/" + orderConfirmResponse.Id);
            var editedReadyOrder = await GetSingularEntity<Order>("api/v1/Order/GetOrder/" + orderConfirmResponse.Id);
            editedReadyOrder.Id.Should().Be(orderConfirmResponse.Id);
            editedReadyOrder.OrderCompletionStatus.Should().Be(EOrderStatus.Ready);

            editedReadyOrder.OrderCompletionStatus = EOrderStatus.Finished;
            await EditEntity(editedReadyOrder, "api/v1/Order/PutOrder/" + editedReadyOrder.Id);
            var editedFinishedOrder = await GetSingularEntity<Order>("api/v1/Order/GetOrder/" + editedReadyOrder.Id);
            editedFinishedOrder.OrderCompletionStatus.Should().Be(EOrderStatus.Finished);
            //Confirm that the one who ordered sees that it's finished
            // GetUserOrders
            AddAuthentication(jwtNormalUser, _client);
            var userOrder = await GetSingularEntity<Order[]>("api/v1/Order/GetUserOrders");
            userOrder.Should().NotBeNull().And.HaveCount(1);
            userOrder[0].Id.Should().Be(editedFinishedOrder.Id);

            AddAuthentication(jwtRestaurantOwner, _client);
            var testRestaurantOrder = await GetSingularEntity<Order[]>("api/v1/Order/GetUserOrders");
            testRestaurantOrder.Should().NotBeNull().And.HaveCount(0);
        }


        private async Task<T> GetSingularEntity<T>(string uri)
        {
            var request = await _client.GetAsync(uri);
            request.StatusCode.Should().Be(StatusCodes.Status200OK);
            var response = JsonHelper.DeserializeWithWebDefaults<T>
                (await request.Content.ReadAsStringAsync())!;
            return response;
        }

        private async Task EditEntity<T>(T entity, string uri)
        {
            entity.Should().NotBeNull();
            var orderEditRequest = await _client.PutAsync(uri, GenerateHttpJsonContent(entity!));
            orderEditRequest.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }


        private async Task<IEnumerable<PaymentType>> GetAllPaymentTypes()
        {
            const string? uri = "/api/v1/PaymentType";
            var paymentTypesRequest = await _client.GetAsync(uri);
            paymentTypesRequest.EnsureSuccessStatusCode();

            var paymentTypesResult = JsonHelper.DeserializeWithWebDefaults<PaymentType[]>
                (await paymentTypesRequest.Content.ReadAsStringAsync())!;
            paymentTypesResult.Should().NotBeNull();
            paymentTypesResult.Length.Should().BeGreaterThan(0);
            return paymentTypesResult;
        }

        private void AddAuthentication(string jwt, HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }

        private StringContent GenerateHttpJsonContent(object obj)
        {
            return new(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }
    }
}