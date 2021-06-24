using AutoMapper;

namespace DAL.APP.EF.Mappers
{
    public class FoodInOrderMapper : BaseMapper<DAL.App.DTO.OrderModels.FoodInOrder, Domain.OrderModels.FoodInOrder>
    {
        public FoodInOrderMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}