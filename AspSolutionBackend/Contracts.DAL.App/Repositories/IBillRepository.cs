using System;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.App.DTO.OrderModels;

namespace Contracts.DAL.App.Repositories
{
    public interface IBillRepository : IBaseRepository<DalAppDTO.Bill>
    {
        Task<DalAppDTO.Bill?> GetBillAccordingToOrderId(Guid orderId, Guid userId);
    }

    public interface IBillCustomRepository<TEntity>
    {
    }
}