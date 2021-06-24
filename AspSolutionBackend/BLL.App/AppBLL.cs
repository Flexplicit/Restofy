using AutoMapper;
using BLL.App.DTO.OrderModels;
using BLL.App.Services;
using BLL.Base;
using BLL.Base.Services;
using Contracts.BLL.App;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App;
using Contracts.DAL.Base.Repositories;


namespace BLL.App
{
    public class AppBLL : BaseBLL<IAppUnitOfWork>, IAppBLL
    {
        protected IMapper Mapper;

        public AppBLL(IAppUnitOfWork uow, IMapper mapper) : base(uow)
        {
            Mapper = mapper;
        }

        public IBillLineService BillLines =>
            GetService<IBillLineService>(() => new BillLineService(Uow, Uow.BillLines, Mapper));


        public IRestaurantService Restaurants =>
            GetService<IRestaurantService>(() => new RestaurantService(Uow, Uow.Restaurants, Mapper));

        

        public IRestaurantSubscriptionService RestaurantSubscriptions =>
            GetService<IRestaurantSubscriptionService>(() =>
                new RestaurantSubscriptionService(Uow, Uow.RestaurantSubscription, Mapper));

        public IBillService Bills =>
            GetService<IBillService>(() => new BillService(Uow, Uow.Bills, Mapper));


        public IContactService Contacts =>
            GetService<IContactService>(() => new ContactService(Uow, Uow.Contacts, Mapper));


        public ICreditCardService CreditCards =>
            GetService<ICreditCardService>(() => new CreditCardService(Uow, Uow.CreditCards, Mapper));

        public IFoodService Food =>
            GetService<IFoodService>(() => new FoodService(Uow, Uow.Foods, Mapper));

        public IFoodInOrderService FoodInOrders =>
            GetService<IFoodInOrderService>(() => new FoodInOrderService(Uow, Uow.FoodInOrder, Mapper));

        public IOrderService Orders =>
            GetService<IOrderService>(() => new OrderService(Uow, Uow.Orders, Mapper));

        public ISubscriptionService Subscriptions =>
            GetService<ISubscriptionService>(() => new SubscriptionService(Uow, Uow.Subscriptions, Mapper));
        
        public ICostService Cost =>
            GetService<ICostService>(() => new CostService(Uow, Uow.Costs, Mapper));

        public IBaseEntityService<BLL.App.DTO.OrderModels.CreditCardInfo, DAL.App.DTO.OrderModels.CreditCardInfo>
            CreditCardInfo =>
            GetService<IBaseEntityService<BLL.App.DTO.OrderModels.CreditCardInfo, DAL.App.DTO.OrderModels.CreditCardInfo
            >>(()
                => new BaseEntityService<IAppUnitOfWork, IBaseRepository<DAL.App.DTO.OrderModels.CreditCardInfo>,
                    BLL.App.DTO.OrderModels.CreditCardInfo, DAL.App.DTO.OrderModels.CreditCardInfo>(Uow,
                    Uow.CreditCardsInfo,
                    new Mappers.BaseMapper<CreditCardInfo, DAL.App.DTO.OrderModels.CreditCardInfo>(Mapper)));


        public IBaseEntityService<BLL.App.DTO.OrderModels.FoodGroup, DAL.App.DTO.OrderModels.FoodGroup> FoodGroup =>
            GetService<IBaseEntityService<BLL.App.DTO.OrderModels.FoodGroup, DAL.App.DTO.OrderModels.FoodGroup>>(()
                => new BaseEntityService<IAppUnitOfWork, IBaseRepository<DAL.App.DTO.OrderModels.FoodGroup>,
                    BLL.App.DTO.OrderModels.FoodGroup, DAL.App.DTO.OrderModels.FoodGroup>(Uow, Uow.FoodGroups,
                    new Mappers.BaseMapper<FoodGroup, DAL.App.DTO.OrderModels.FoodGroup>(Mapper)));

        public IBaseEntityService<BLL.App.DTO.OrderModels.PaymentType, DAL.App.DTO.OrderModels.PaymentType>
            PaymentType =>
            GetService<IBaseEntityService<BLL.App.DTO.OrderModels.PaymentType, DAL.App.DTO.OrderModels.PaymentType>>(()
                => new BaseEntityService<IAppUnitOfWork, IBaseRepository<DAL.App.DTO.OrderModels.PaymentType>,
                    BLL.App.DTO.OrderModels.PaymentType, DAL.App.DTO.OrderModels.PaymentType>(Uow, Uow.PaymentTypes,
                    new Mappers.BaseMapper<PaymentType, DAL.App.DTO.OrderModels.PaymentType>(Mapper)));

        public IBaseEntityService<ContactType, DAL.App.DTO.OrderModels.ContactType> ContactType =>
            GetService<IBaseEntityService<BLL.App.DTO.OrderModels.ContactType, DAL.App.DTO.OrderModels.ContactType>>(()
                => new BaseEntityService<IAppUnitOfWork, IBaseRepository<DAL.App.DTO.OrderModels.ContactType>,
                    BLL.App.DTO.OrderModels.ContactType, DAL.App.DTO.OrderModels.ContactType>(Uow, Uow.ContactTypes,
                    new Mappers.BaseMapper<ContactType, DAL.App.DTO.OrderModels.ContactType>(Mapper)));
    }
}