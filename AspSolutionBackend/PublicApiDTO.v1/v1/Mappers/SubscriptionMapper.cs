using AutoMapper;
using BLL.App.DTO.OrderModels;

namespace PublicApiDTO.v1.v1.Mappers
{
    public class SubscriptionMapper : BaseApiMapper<PublicApiDTO.v1.v1.OrderModels.Subscription,
        BLL.App.DTO.OrderModels.Subscription>
    {
        public SubscriptionMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}