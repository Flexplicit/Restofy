using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO.OrderModels;
using DALAppDTO = DAL.App.DTO.OrderModels;

namespace Contracts.BLL.App.Services
{
    public interface IContactService : IBaseEntityService<BLLAppDTO.Contact, DALAppDTO.Contact>,
        IContactCustomRepository<BLLAppDTO.Contact>
    {
        // Task<BLLAppDTO.Contact?> FirstOrDefaultAsync(Guid id, Guid userId, bool noTracking);
        Task<IEnumerable<BLLAppDTO.Contact?>> GetAllAsync(Guid userId, Guid? restaurantId, bool noTracking = true);

        // new Task<BLLAppDTO.Contact> Add(BLLAppDTO.Contact entity);
    }
}