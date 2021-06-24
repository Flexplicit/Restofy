using AutoMapper;
using DAL.APP.EF.Mappers;

namespace BLL.App.Mappers
{
    public class BillMapper : BaseMapper<BLL.App.DTO.OrderModels.Bill, DAL.App.DTO.OrderModels.Bill>
    {
        public BillMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}