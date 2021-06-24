using System;
using System.Threading.Tasks;
using Contracts.DAL.Base;

namespace Contracts.BLL.Base
{
    public interface IBaseBLL
    {
        Task<int> SaveChangesTask();
        
        TService GetService<TService>(Func<TService> serviceCreationMethod)
            where TService : class;
    }
}