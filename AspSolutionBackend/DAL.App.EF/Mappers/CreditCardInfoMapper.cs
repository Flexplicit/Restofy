using AutoMapper;

namespace DAL.APP.EF.Mappers
{
    public class CreditCardInfoMapper : BaseMapper<DAL.App.DTO.OrderModels.CreditCardInfo, Domain.OrderModels.CreditCardInfo>
    {
        public CreditCardInfoMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}