using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.App.DTO.OrderModels;


namespace Contracts.DAL.App.Repositories
{
    public interface ICreditCardsRepository: IBaseRepository<DalAppDTO.CreditCard>, ICreditCardCustomRepository<DalAppDTO.CreditCard>
    {
        
    }
    
    public interface ICreditCardCustomRepository<TEntity>
    {
        
    }
}