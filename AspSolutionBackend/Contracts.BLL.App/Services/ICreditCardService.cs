using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO.OrderModels;
using DALAppDTO = DAL.App.DTO.OrderModels;
namespace Contracts.BLL.App.Services
{
    public interface ICreditCardService : IBaseEntityService<BLLAppDTO.CreditCard, DALAppDTO.CreditCard>, ICreditCardCustomRepository<BLLAppDTO.CreditCard>
    {
    }
}