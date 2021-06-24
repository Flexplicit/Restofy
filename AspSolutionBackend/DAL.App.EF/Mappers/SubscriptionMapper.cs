using AutoMapper;

namespace DAL.APP.EF.Mappers
{
    public class SubscriptionMapper : BaseMapper<DAL.App.DTO.OrderModels.Subscription, Domain.OrderModels.Subscription>
    {
        public SubscriptionMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}