using AutoMapper;

namespace BLL.App.Mappers
{
    public class RestaurantSubscriptionMapper : BaseMapper<BLL.App.DTO.OrderModels.RestaurantSubscription,
        DAL.App.DTO.OrderModels.RestaurantSubscription>
    {
        public RestaurantSubscriptionMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}