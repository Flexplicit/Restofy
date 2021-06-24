using System;
using AutoMapper;
using PublicDTO = PublicApiDTO.v1.v1.OrderModels;
using BllAppDTO = BLL.App.DTO.OrderModels;

namespace PublicApiDTO.v1.v1.Mappers
{
    public class
        RestaurantSubscriptionMapper : BaseApiMapper<PublicDTO.RestaurantSubscription, BllAppDTO.RestaurantSubscription>
    {
        public RestaurantSubscriptionMapper(IMapper mapper) : base(mapper)
        {
        }

        public static BllAppDTO.RestaurantSubscription ApiToBllRestaurantSubscriptionCreate(
            PublicDTO.RestaurantSubscriptionCreate createSub)
        {
            return new()
            {
                RestaurantId = createSub.RestaurantId,
                PaypalId = createSub.PaypalId,
                SubscriptionId = createSub.SubscriptionId,
                IsPaid = createSub.IsPaid
            };
        }
    }
}