using AutoMapper;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using Contracts.DAL.Base.Repositories;
using DAL.APP.EF.Mappers;
using DAL.APP.EF.Repositories;
using DAL.Base.EF;
using DAL.Base.EF.Repositories;
using Domain.OrderModels;
using DalAppDTO = DAL.App.DTO.OrderModels;

namespace DAL.APP.EF
{
    public class AppUnitOfWork : BaseUnitOfWork<AppDbContext>, IAppUnitOfWork
    {
        protected IMapper Mapper;

        public AppUnitOfWork(AppDbContext uowDbContext, IMapper mapper) : base(uowDbContext)
        {
            Mapper = mapper;
        }


        public IRestaurantRepository Restaurants =>
            GetRepository(() => new RestaurantRepository(_uowContext, Mapper));

        public IFoodRepository Foods =>
            GetRepository(() => new FoodRepository(_uowContext, Mapper));

        public IBillRepository Bills =>
            GetRepository(() => new BillRepository(_uowContext, Mapper));

        public IContactRepository Contacts =>
            GetRepository(() => new ContactRepository(_uowContext, Mapper));

        public IFoodInOrderRepository FoodInOrder =>
            GetRepository(() => new FoodInOrderRepository(_uowContext, Mapper));

        public IOrderRepository Orders =>
            GetRepository(() => new OrderRepository(_uowContext, Mapper));
        public IRestaurantSubscriptionsRepository RestaurantSubscription =>
            GetRepository(() => new RestaurantSubscriptionsRepository(_uowContext, Mapper));

        public ICreditCardsRepository CreditCards =>
            GetRepository(() => new CreditCardsRepository(_uowContext, Mapper));
        
        public ICostRepository Costs =>
            GetRepository(() => new CostRepository(_uowContext, Mapper));

        // public IBaseRepository<AppUser> AppUsers =>
        //     GetRepository(() => new BaseRepository<AppUser, AppDbContext>(_uowContext));

        public IBillLinesRepository BillLines =>
            GetRepository(() => new BillLinesRepository(_uowContext, Mapper));

        public ISubscriptionRepository Subscriptions =>
            GetRepository(() => new SubscriptionRepository(_uowContext, Mapper));

        public IBaseRepository<DalAppDTO.CreditCardInfo> CreditCardsInfo =>
            GetRepository(() =>
                new BaseRepository<DalAppDTO.CreditCardInfo, Domain.OrderModels.CreditCardInfo, AppDbContext>(
                    _uowContext, new BaseMapper<DalAppDTO.CreditCardInfo, CreditCardInfo>(Mapper)));

        public IBaseRepository<DalAppDTO.PaymentType> PaymentTypes =>
            GetRepository(() =>
                new BaseRepository<DalAppDTO.PaymentType, Domain.OrderModels.PaymentType, AppDbContext>(_uowContext,
                    new BaseMapper<DalAppDTO.PaymentType, PaymentType>(Mapper)));

        public IBaseRepository<DalAppDTO.FoodGroup> FoodGroups =>
            GetRepository(() =>
                new BaseRepository<DalAppDTO.FoodGroup, Domain.OrderModels.FoodGroup, AppDbContext>(_uowContext,
                    new BaseMapper<DalAppDTO.FoodGroup, FoodGroup>(Mapper)));
        public IBaseRepository<DalAppDTO.ContactType> ContactTypes =>
            GetRepository(() =>
                new BaseRepository<DalAppDTO.ContactType, Domain.OrderModels.ContactType, AppDbContext>(_uowContext,
                    new BaseMapper<DalAppDTO.ContactType, ContactType>(Mapper)));
    }
}