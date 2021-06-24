using System;
using System.Threading.Tasks;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO.OrderModels;
using DALAppDTO = DAL.App.DTO.OrderModels;

namespace Contracts.BLL.App.Services
{
    public interface IBillService : IBaseEntityService<BLLAppDTO.Bill, DALAppDTO.Bill>,
        IBillCustomRepository<BLLAppDTO.Bill>
    {
        Task<BLLAppDTO.Bill> GenerateBillAndBillLineForOrder(Guid orderId,  Guid userId = default);
        
        Task<BLLAppDTO.Bill?> GetBillAccordingToOrderId(Guid orderId, Guid userId);

    }
}