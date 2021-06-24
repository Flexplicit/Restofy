using AutoMapper;
using PublicApiDTO.v1.v1.OrderModels;

namespace PublicApiDTO.v1.v1.Mappers
{
    public class FoodInOrderMapper : BaseApiMapper<FoodInOrder, BLL.App.DTO.OrderModels.FoodInOrder>
    {
        public FoodInOrderMapper(IMapper mapper) : base(mapper)
        {
        }

        public static BLL.App.DTO.OrderModels.FoodInOrder ApiFoodInOrderCreateToBll(FoodInOrderCreate foodCreate)
        {
            return new()
            {
                Amount = foodCreate.Amount,
                FoodId = foodCreate.FoodId,
            };
        }

        public static PublicApiDTO.v1.v1.OrderModels.FoodInOrderWithFood BllToApiFoodInOrderExtra(
            BLL.App.DTO.OrderModels.FoodInOrder foodInOrder)
        {
            return new()
            {
                Id = foodInOrder.Id,
                Amount = foodInOrder.Amount,
                Food = FoodMapper.MapFoodFromBllToApi(foodInOrder.Food!),
                FoodId = foodInOrder.FoodId,
                OrderId = foodInOrder.OrderId
            };
        }
    }
}