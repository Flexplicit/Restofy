using AutoMapper;

namespace PublicApiDTO.v1.v1.Mappers
{
    public class FoodMapper : BaseApiMapper<PublicApiDTO.v1.v1.OrderModels.Food, BLL.App.DTO.OrderModels.Food>
    {
        public FoodMapper(IMapper mapper) : base(mapper)
        {
        }

        public static PublicApiDTO.v1.v1.OrderModels.Food MapFoodFromBllToApi(BLL.App.DTO.OrderModels.Food food)
        {
            return new()
            {
                Description = food.Description,
                Id = food.Id,
                CostId = food.CostId,
                CostWithVat = food.Cost?.CostWithVat ?? 0,
                FoodGroupId = food.FoodGroupId,
                FoodName = food.FoodName,
                Picture = food.Picture,
                RestaurantId = food.RestaurantId,
                FoodGroup = food.FoodGroup!.FoodGroupType.ToString()
            };
        }

        public static BLL.App.DTO.OrderModels.Food MapFoodCreatePublicToBll(
            PublicApiDTO.v1.v1.OrderModels.FoodCreate foodCreate)
        {
            return new()
            {
                Description = foodCreate.Description,
                FoodName = foodCreate.FoodName,
                Picture = foodCreate.Picture,
                FoodGroupId = foodCreate.FoodGroupId!.Value,
                RestaurantId = foodCreate.RestaurantId!.Value
            };
        }

        public static BLL.App.DTO.OrderModels.Food MapFoodEditPublicToBll(
            PublicApiDTO.v1.v1.OrderModels.FoodEdit foodEdit)
        {
            return new()
            {
                Id = foodEdit.Id,
                Description = foodEdit.Description,
                FoodName = foodEdit.FoodName,
                Picture = foodEdit.Picture,
                FoodGroupId = foodEdit.FoodGroupId!.Value,
                RestaurantId = foodEdit.RestaurantId!.Value,
                // CostId = foodEdit.CostId!.Value,
            };
        }
    }
}