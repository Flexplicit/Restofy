using AutoMapper;

namespace BLL.App.Mappers
{
    public class CostMapper : BaseMapper<BLL.App.DTO.OrderModels.Cost, DAL.App.DTO.OrderModels.Cost>
    {
        public CostMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}