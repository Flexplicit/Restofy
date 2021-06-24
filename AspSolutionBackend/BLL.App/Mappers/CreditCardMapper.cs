using AutoMapper;
using DAL.APP.EF.Mappers;

namespace BLL.App.Mappers
{
    public class CreditCardMapper: BaseMapper<BLL.App.DTO.OrderModels.CreditCard, DAL.App.DTO.OrderModels.CreditCard>
    {
        public CreditCardMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}