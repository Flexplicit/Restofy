using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.App.DTO.OrderModels;

namespace Contracts.DAL.App.Repositories
{
    public interface IBillLinesRepository : IBaseRepository<DalAppDTO.BillLine>,
        IBillLinesCustomRepository<DalAppDTO.BillLine>
    {
        Task<IEnumerable<DalAppDTO.BillLine>> GetAllBillLinesAccordingToBillId(Guid userId, Guid billId);
    }

    public interface IBillLinesCustomRepository<TEntity>
    {
    }
}