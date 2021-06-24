using AutoMapper;

namespace DAL.APP.EF.Mappers
{
    public class RestaurantMapper : BaseMapper<DAL.App.DTO.OrderModels.Restaurant, Domain.OrderModels.Restaurant>
    {
        public RestaurantMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}