using Contracts.BLL.App.Services;
using Contracts.BLL.Base;
using Contracts.BLL.Base.Services;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.BLL.App
{
    public interface IAppBLL : IBaseBLL
    {
        public IBillLineService BillLines { get; }
        public IBillService Bills { get; }
        public IContactService Contacts { get; }
        public ICreditCardService CreditCards { get; }
        public IFoodInOrderService FoodInOrders { get; }
        public IFoodService Food { get; }
        public IOrderService Orders { get; }
        public IRestaurantService Restaurants { get; }
        public IRestaurantSubscriptionService RestaurantSubscriptions { get; }
        public ISubscriptionService Subscriptions { get; }

        public ICostService Cost { get; }

        public IBaseEntityService<BLLAppDTO.OrderModels.CreditCardInfo, DALAppDTO.OrderModels.CreditCardInfo>
            CreditCardInfo { get; }

        public IBaseEntityService<BLLAppDTO.OrderModels.FoodGroup, DALAppDTO.OrderModels.FoodGroup> FoodGroup { get; }

        public IBaseEntityService<BLLAppDTO.OrderModels.PaymentType, DALAppDTO.OrderModels.PaymentType> PaymentType
        {
            get;
        }

        public IBaseEntityService<BLLAppDTO.OrderModels.ContactType, DALAppDTO.OrderModels.ContactType> ContactType
        {
            get;
        }
    }
}