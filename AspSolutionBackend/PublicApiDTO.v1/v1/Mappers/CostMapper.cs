using AutoMapper;
using PublicApiDTO.v1.v1.OrderModels;

namespace PublicApiDTO.v1.v1.Mappers
{
    public class CostMapper: BaseApiMapper<Cost, BLL.App.DTO.OrderModels.Cost>
    {
        public CostMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}