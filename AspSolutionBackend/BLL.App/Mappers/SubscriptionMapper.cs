using AutoMapper;
using DAL.APP.EF.Mappers;

namespace BLL.App.Mappers
{
    public class SubscriptionMapper: BaseMapper<BLL.App.DTO.OrderModels.Subscription, DAL.App.DTO.OrderModels.Subscription>
    {
        public SubscriptionMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}