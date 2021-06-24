using AutoMapper;

namespace DAL.APP.EF.Mappers
{
    public class BillLineMapper : BaseMapper<DAL.App.DTO.OrderModels.BillLine, Domain.OrderModels.BillLine>
    {
        public BillLineMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}