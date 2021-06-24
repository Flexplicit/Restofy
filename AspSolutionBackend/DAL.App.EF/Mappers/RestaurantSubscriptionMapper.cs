using AutoMapper;

namespace DAL.APP.EF.Mappers
{
    public class RestaurantSubscriptionMapper : BaseMapper<DAL.App.DTO.OrderModels.RestaurantSubscription, Domain.OrderModels.RestaurantSubscription>
    {
        public RestaurantSubscriptionMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}