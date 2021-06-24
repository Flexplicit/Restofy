using AutoMapper;
using PublicApiDTO.v1.v1.OrderModels;

namespace PublicApiDTO.v1.v1.Mappers.MappingProfiles
{
    public class BillLineMapper: BaseApiMapper<BillLine,BLL.App.DTO.OrderModels.BillLine>
    {
        public BillLineMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}