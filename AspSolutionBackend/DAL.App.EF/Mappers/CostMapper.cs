using AutoMapper;

namespace DAL.APP.EF.Mappers
{
    public class CostMapper : BaseMapper<DAL.App.DTO.OrderModels.Cost, Domain.OrderModels.Cost>
    {
        public CostMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}