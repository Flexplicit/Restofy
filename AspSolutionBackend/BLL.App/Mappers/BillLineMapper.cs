
using AutoMapper;

namespace BLL.App.Mappers
{
    public class BillLineMapper : BaseMapper<BLL.App.DTO.OrderModels.BillLine, DAL.App.DTO.OrderModels.BillLine>
    {
        public BillLineMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}