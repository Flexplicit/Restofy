using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.DTO.OrderModels;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;

namespace BLL.App.Services
{
    public class RestaurantService :
        BaseEntityService<IAppUnitOfWork, IRestaurantRepository, BLLAppDTO.OrderModels.Restaurant,
            DALAppDTO.OrderModels.Restaurant, Guid>, IRestaurantService
    {
        public RestaurantService(IAppUnitOfWork serviceUow, IRestaurantRepository serviceRepository, IMapper mapper) :
            base(serviceUow, serviceRepository, new RestaurantMapper(mapper))
        {
        }

        public async Task<BLLAppDTO.OrderModels.Restaurant?> GetRestaurantWithMenuAsync(Guid id, Guid userId = default,
            bool noTracking = false)
        {
            return AddSubscriptionData(
                (Mapper.Map(await ServiceRepository.GetRestaurantWithMenuAsync(id, userId, noTracking))!));
        }

        public async Task<IEnumerable<Restaurant>> GetMyRestaurantsAsync(Guid userId = default, bool noTracking = true)
        {
            return (await ServiceRepository.GetMyRestaurants(userId, noTracking))!
                .Select(x => AddSubscriptionData(Mapper.Map(x)))!;
        }

        // Remove all restaurants that dont have active subscription
        public new async Task<IEnumerable<Restaurant>> GetAllAsync(Guid userId, bool noTracking)
        {
            var repoRes = (await ServiceRepository.GetAllAsync(userId))
                .Select(x => AddSubscriptionData(Mapper.Map(x)))!.ToList();

            repoRes.RemoveAll(x => !x!.IsValidSubscription);
            return repoRes!;
        }


        public new async Task<Restaurant?> FirstOrDefaultAsync(Guid id, Guid userId, bool noTracking)
        {
            return AddSubscriptionData(Mapper.Map(await ServiceRepository.FirstOrDefaultAsync(id, userId, noTracking)));
        }

        private static Restaurant? AddSubscriptionData(Restaurant? restaurant)
        {
            if (restaurant == null || (restaurant.RestaurantSubscriptions?.Count ?? 0) == 0)
            {
                return restaurant;
            }

            foreach (var sub in restaurant!.RestaurantSubscriptions!)
            {
                if (sub.ActiveUntill == null || sub.ActiveUntill.Value.CompareTo(DateTime.Now) < 0) continue;
                restaurant.IsValidSubscription = true;
                restaurant.SubscriptionDaysLeft = sub.ActiveUntill.Value.Subtract(DateTime.Now).Days;
            }

            return restaurant;
        }


        public new async Task<Restaurant> Remove(Restaurant entity, Guid userId = default)
        {
            var res = await ServiceRepository.GetRestaurantWithOrderData(entity.Id, userId, true);
            foreach (var order in res!.RestaurantOrder!)
            {
                foreach (var foodInOrder in order!.FoodInOrders!)
                {
                    foodInOrder.OrderId = default;
                    foodInOrder.Order = null;
                    foodInOrder.Food = null;
                    ServiceUow.FoodInOrder.Remove(foodInOrder);
                }

                order.Restaurant = null;
                order.FoodInOrders = null;
                ServiceUow.Orders.Remove(order);
            }

            return Mapper.Map(ServiceRepository.Remove(Mapper.Map(entity)!))!;
        }
    }
}