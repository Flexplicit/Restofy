using AutoMapper;
using DAL.APP.EF.Mappers;

namespace BLL.App.Mappers
{
    public class FoodMapper: BaseMapper<BLL.App.DTO.OrderModels.Food, DAL.App.DTO.OrderModels.Food>
    {
        public FoodMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}