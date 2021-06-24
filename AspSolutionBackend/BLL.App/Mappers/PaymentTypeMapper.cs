using AutoMapper;

namespace BLL.App.Mappers
{
    public class PaymentTypeMapper: BaseMapper<BLL.App.DTO.OrderModels.PaymentType, DAL.App.DTO.OrderModels.PaymentType>
    {
        public PaymentTypeMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}