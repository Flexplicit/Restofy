using AutoMapper;
using DAL.APP.EF.Mappers;

namespace BLL.App.Mappers
{
    public class OrderMapper: BaseMapper<BLL.App.DTO.OrderModels.Order, DAL.App.DTO.OrderModels.Order>
    {
        public OrderMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}