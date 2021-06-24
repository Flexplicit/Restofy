using System;


namespace PublicApiDTO.v1.v1.OrderModels
{
    public class CreditCard : CreditCardCreate
    {
        public Guid Id { get; set; }
    }

    public class CreditCardCreate
    {
        public CreditCardInfoCreate CreditCardInfo { get; set; } = null!;
    }
}