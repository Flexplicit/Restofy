using AutoMapper;
using DAL.APP.EF.Mappers;

namespace BLL.App.Mappers
{
    public class RestaurantMapper : BaseMapper<BLL.App.DTO.OrderModels.Restaurant, DAL.App.DTO.OrderModels.Restaurant>
    {
        public RestaurantMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}