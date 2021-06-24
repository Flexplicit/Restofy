using System;
using AutoMapper;
using BLL.App.DTO.OrderModels;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base.Mappers;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;

namespace BLL.App.Services
{
    public class SubscriptionService :
        BaseEntityService<IAppUnitOfWork, ISubscriptionRepository, BLLAppDTO.OrderModels.Subscription,
            DALAppDTO.OrderModels.Subscription, Guid>, ISubscriptionService
    {
        public SubscriptionService(IAppUnitOfWork serviceUow, ISubscriptionRepository serviceRepository,
            IMapper mapper) : base(serviceUow, serviceRepository, new SubscriptionMapper(mapper))
        {
        }
    }
}