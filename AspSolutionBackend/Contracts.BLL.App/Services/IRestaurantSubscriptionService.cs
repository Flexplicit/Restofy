using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO.OrderModels;
using DALAppDTO = DAL.App.DTO.OrderModels;

namespace Contracts.BLL.App.Services
{
    public interface IRestaurantSubscriptionService : IBaseEntityService<BLLAppDTO.RestaurantSubscription,DALAppDTO.RestaurantSubscription>,
        IRestaurantSubscriptionsCustomRepository<BLLAppDTO.RestaurantSubscription>
    {
        
    }
}

