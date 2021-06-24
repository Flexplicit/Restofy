using AutoMapper;
using PublicApiDTO.v1.v1.OrderModels;

namespace PublicApiDTO.v1.v1.Mappers
{
    public class OrderMapper : BaseApiMapper<Order, BLL.App.DTO.OrderModels.Order>
    {
        public OrderMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}