using AutoMapper;

namespace PublicApiDTO.v1.v1.Mappers
{
    public class PaymentTypeMapper : BaseApiMapper<PublicApiDTO.v1.v1.OrderModels.PaymentType,BLL.App.DTO.OrderModels.PaymentType>
    {
        public PaymentTypeMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}