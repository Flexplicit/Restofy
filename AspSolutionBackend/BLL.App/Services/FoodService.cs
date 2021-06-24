using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.DTO.OrderModels;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base.Mappers;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;
using Food = DAL.App.DTO.OrderModels.Food;

namespace BLL.App.Services
{
    public class FoodService :
        BaseEntityService<IAppUnitOfWork, IFoodRepository, BLLAppDTO.OrderModels.Food,
            DALAppDTO.OrderModels.Food, Guid>, IFoodService
    {
        public FoodService(IAppUnitOfWork serviceUow, IFoodRepository serviceRepository, IMapper mapper) : base(
            serviceUow, serviceRepository, new FoodMapper(mapper))
        {
        }


        public async Task<IEnumerable<BLLAppDTO.OrderModels.Food>> GetRestaurantsFoodAsync(Guid restaurantId,
            bool noTracking = true)
        {
            return (await ServiceRepository.GetRestaurantsFoodAsync(restaurantId)).Select(x => Mapper.Map(x))!;
        }

        public new async Task<BLLAppDTO.OrderModels.Food> Remove(BLLAppDTO.OrderModels.Food entity,
            Guid userId = default)
        {
            var res = Mapper.Map(entity);
            // var res = await ServiceRepository.FirstOrDefaultAsync(entity.Id, userId, true);
            foreach (var food in res!.FoodInOrders!)
            {
                food.Food = null;
                ServiceUow.FoodInOrder.Remove(food);
            }

            // ServiceUow.Costs.Remove(res.Cost!);
            // return entity;
            entity.FoodGroup = null;
            entity.Cost = null;

            entity.FoodInOrders = null;
            return Mapper.Map(ServiceRepository.Remove(Mapper.Map(entity)!))!;
        }


        public Task<IEnumerable<Food>> GetFoodCollectionWithExtraInfoAsync(IEnumerable<Guid> foodItemsId)
        {
            throw new NotImplementedException();
        }
    }
}