using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.App.DTO.OrderModels;

namespace Contracts.DAL.App.Repositories
{
    public interface ICostRepository :  IBaseRepository<DalAppDTO.Cost>, ICostCustomRepository<DalAppDTO.Cost>
    {
        
    }

    public interface ICostCustomRepository<TEntity>
    {
        
    }
}