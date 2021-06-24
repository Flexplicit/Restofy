using AutoMapper;
using PublicDTO = PublicApiDTO.v1.v1.OrderModels;
using BllAppDTO = BLL.App.DTO.OrderModels;

namespace PublicApiDTO.v1.v1.Mappers
{
    public class RestaurantMapper : BaseApiMapper<PublicDTO.Restaurant, BllAppDTO.Restaurant>
    {
        public RestaurantMapper(IMapper mapper) : base(mapper)
        {
        }

        public static BllAppDTO.Restaurant ApiEditRestaurantToBll(PublicDTO.RestaurantEdit restaurant)
        {
            var bllRestaurant = new BllAppDTO.Restaurant()
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                RestaurantAddress = restaurant.RestaurantAddress,
                Description = restaurant.Description,
                Picture = restaurant.Picture
            };
            return bllRestaurant;
        }

        public static BllAppDTO.Restaurant ApiCreateRestaurantToBll(PublicDTO.RestaurantCreate restaurant)
        {
            var bllRestaurant = new BllAppDTO.Restaurant()
            {
                Name = restaurant.Name,
                RestaurantAddress = restaurant.RestaurantAddress,
                Description = restaurant.Description,
                Picture = restaurant.Picture
            };
            return bllRestaurant;
        }
    }
}