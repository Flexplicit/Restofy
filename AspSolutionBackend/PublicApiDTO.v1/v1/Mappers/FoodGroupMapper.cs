using AutoMapper;

namespace PublicApiDTO.v1.v1.Mappers
{
    public class
        FoodGroupMapper : BaseApiMapper<PublicApiDTO.v1.v1.OrderModels.FoodGroup, BLL.App.DTO.OrderModels.FoodGroup>
    {
        public FoodGroupMapper(IMapper mapper) : base(mapper)
        {
        }

        public static PublicApiDTO.v1.v1.OrderModels.FoodGroup MapFoodGroupFromBllToPublicApiString(
            BLL.App.DTO.OrderModels.FoodGroup foodGroup)
        {
            return new()
            {
                Id = foodGroup.Id,
                FoodGroupType = foodGroup.FoodGroupType.ToString()
            };
        }
    }
}