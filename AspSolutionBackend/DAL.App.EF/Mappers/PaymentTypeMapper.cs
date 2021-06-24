using AutoMapper;

namespace DAL.APP.EF.Mappers
{
    public class PaymentTypeMapper : BaseMapper<DAL.App.DTO.OrderModels.PaymentType, Domain.OrderModels.PaymentType>
    {
        public PaymentTypeMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}