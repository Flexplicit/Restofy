using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class ContactService : BaseEntityService<IAppUnitOfWork, IContactRepository, BLLAppDTO.OrderModels.Contact,
        DALAppDTO.OrderModels.Contact, Guid>, IContactService

    {
        public ContactService(IAppUnitOfWork serviceUow, IContactRepository serviceRepository, IMapper mapper) : base(
            serviceUow, serviceRepository, new ContactMapper(mapper))
        {
        }

        public async Task<IEnumerable<Contact?>> GetAllAsync(Guid userId, Guid? restaurantId, bool noTracking)
        {
            return (await ServiceRepository.GetAllUserOrRestaurantContactsAsync(userId, restaurantId, noTracking))
                .Select(x => Mapper.Map(x));
        }
    }
}