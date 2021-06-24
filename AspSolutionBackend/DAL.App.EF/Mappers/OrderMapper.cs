using AutoMapper;

namespace DAL.APP.EF.Mappers
{
    public class OrderMapper : BaseMapper<DAL.App.DTO.OrderModels.Order, Domain.OrderModels.Order>
    {
        public OrderMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}