using AutoMapper;
using DAL.APP.EF.Mappers;

namespace DAL.APP.EF.Mappers
{
    public class CreditCardMapper : BaseMapper<DAL.App.DTO.OrderModels.CreditCard, Domain.OrderModels.CreditCard>
    {
        public CreditCardMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}