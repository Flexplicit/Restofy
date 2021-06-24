using AutoMapper;
using DAL.APP.EF.Mappers;

namespace BLL.App.Mappers
{
    public class FoodInOrderMapper: BaseMapper<BLL.App.DTO.OrderModels.FoodInOrder, DAL.App.DTO.OrderModels.FoodInOrder>
    {
        public FoodInOrderMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}