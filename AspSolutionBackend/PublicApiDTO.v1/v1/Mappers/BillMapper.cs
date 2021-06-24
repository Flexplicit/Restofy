using AutoMapper;
using PublicApiDTO.v1.v1.OrderModels;

namespace PublicApiDTO.v1.v1.Mappers
{
    public class BillMapper : BaseApiMapper<Bill, BLL.App.DTO.OrderModels.Bill>
    {
        public BillMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}