using System.Threading.Tasks;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO.OrderModels;
using DALAppDTO = DAL.App.DTO.OrderModels;

namespace Contracts.BLL.App.Services
{
    public interface ICostService : IBaseEntityService<BLLAppDTO.Cost, DALAppDTO.Cost>,
        ICostCustomRepository<BLLAppDTO.Cost>
    {
        BLLAppDTO.Cost GenerateBllCostFromDecimal(decimal costWithVat);
    }
}