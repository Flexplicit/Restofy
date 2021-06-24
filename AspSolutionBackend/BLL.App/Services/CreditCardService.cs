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
    public class CreditCardService : BaseEntityService<IAppUnitOfWork, ICreditCardsRepository,
        BLLAppDTO.OrderModels.CreditCard, DALAppDTO.OrderModels.CreditCard, Guid>, ICreditCardService
    {
        public CreditCardService(IAppUnitOfWork serviceUow, ICreditCardsRepository serviceRepository,
            IMapper mapper) : base(serviceUow, serviceRepository, new CreditCardMapper(mapper))
        {
        }
    }
}