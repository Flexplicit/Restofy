using AutoMapper;

namespace DAL.APP.EF.Mappers
{
    public class FoodGroupMapper: BaseMapper<DAL.App.DTO.OrderModels.FoodGroup, Domain.OrderModels.FoodGroup>
    {
        public FoodGroupMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}