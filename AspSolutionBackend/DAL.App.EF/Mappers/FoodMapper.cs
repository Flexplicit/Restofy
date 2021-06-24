using AutoMapper;

namespace DAL.APP.EF.Mappers
{
    public class FoodMapper : BaseMapper<DAL.App.DTO.OrderModels.Food, Domain.OrderModels.Food>
    {
        public FoodMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}