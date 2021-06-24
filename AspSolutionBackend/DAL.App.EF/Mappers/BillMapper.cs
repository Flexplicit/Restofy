using AutoMapper;

namespace DAL.APP.EF.Mappers
{
    public class BillMapper: BaseMapper<DAL.App.DTO.OrderModels.Bill, Domain.OrderModels.Bill>
    {
        public BillMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}