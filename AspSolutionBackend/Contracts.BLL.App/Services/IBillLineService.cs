using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO.OrderModels;
using DALAppDTO = DAL.App.DTO.OrderModels;

namespace Contracts.BLL.App.Services
{
    public interface IBillLineService : IBaseEntityService<BLLAppDTO.BillLine, DALAppDTO.BillLine>,
        IBillLinesCustomRepository<BLLAppDTO.BillLine>
    {
        Task<IEnumerable<BLLAppDTO.BillLine>> GetAllBillLinesAccordingToBillId(Guid userId, Guid billId);
    }
}