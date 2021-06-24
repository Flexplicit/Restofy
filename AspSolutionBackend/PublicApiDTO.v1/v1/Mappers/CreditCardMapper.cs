using AutoMapper;
using PublicApiDTO.v1.v1.OrderModels;
using BllAppDTO = BLL.App.DTO.OrderModels;
using PublicDTO = PublicApiDTO.v1;

namespace PublicApiDTO.v1.v1.Mappers
{
    public class CreditCardMapper : BaseApiMapper<CreditCard, BLL.App.DTO.OrderModels.CreditCard>
    {
        public CreditCardMapper(IMapper mapper) : base(mapper)
        {
        }

        public static CreditCard MapBllToPublicApiCreditCard(BLL.App.DTO.OrderModels.CreditCard creditCard)
        {
            return new()
            {
                Id = creditCard.Id,
                CreditCardInfo = new CreditCardInfo()
                {
                    CreditCardNumber = creditCard.CreditCardInfo!.CreditCardNumber,
                    Cvc = creditCard.CreditCardInfo.Cvc,
                    ExpiryDate = creditCard.CreditCardInfo.ExpiryDate
                }
            };
        }

        public static BLL.App.DTO.OrderModels.CreditCard MapPublicToBllCreditCardCreate(CreditCardCreate creditCard)
        {
            return new()
            {
                CreditCardInfo = new BllAppDTO.CreditCardInfo()
                {
                    CreditCardNumber = creditCard.CreditCardInfo!.CreditCardNumber,
                    Cvc = creditCard.CreditCardInfo.Cvc,
                    ExpiryDate = creditCard.CreditCardInfo.ExpiryDate
                }
            };
        }

        public static BLL.App.DTO.OrderModels.CreditCard MapPublicToBllCreditCard(CreditCard creditCard)
        {
            return new()
            {
                Id = creditCard.Id,
                CreditCardInfo = new BllAppDTO.CreditCardInfo()
                {
                    CreditCardNumber = creditCard.CreditCardInfo!.CreditCardNumber,
                    Cvc = creditCard.CreditCardInfo.Cvc,
                    ExpiryDate = creditCard.CreditCardInfo.ExpiryDate
                }
            };
        }
    }
}