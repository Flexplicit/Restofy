using Contracts.DAL.App.Repositories;
using Contracts.DAL.Base;
using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.App.DTO.OrderModels;


namespace Contracts.DAL.App
{
    public interface IAppUnitOfWork : IBaseUnitOfWork
    {
        IRestaurantRepository Restaurants { get; }
        IFoodRepository Foods { get; }

        IBillRepository Bills { get; }

        IContactRepository Contacts { get; }
        ICostRepository Costs { get; }

        IFoodInOrderRepository FoodInOrder { get; }

        IOrderRepository Orders { get; }

        IRestaurantSubscriptionsRepository RestaurantSubscription { get; }
        ISubscriptionRepository Subscriptions { get; }

        ICreditCardsRepository CreditCards { get; }

        // IBaseRepository<AppUser> AppUsers { get; }
        IBillLinesRepository BillLines { get; }
        IBaseRepository<DalAppDTO.CreditCardInfo> CreditCardsInfo { get; }
        IBaseRepository<DalAppDTO.PaymentType> PaymentTypes { get; }
        IBaseRepository<DalAppDTO.FoodGroup> FoodGroups { get; }


        IBaseRepository<DalAppDTO.ContactType> ContactTypes { get; }
    }
}