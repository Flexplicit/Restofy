using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL.Base.EF
{
    public class BaseUnitOfWork<TDbContext>: BaseUnitOfWork
    where TDbContext : DbContext
    {
        protected readonly TDbContext _uowContext;
        
        public BaseUnitOfWork(TDbContext ctx)
        {
            _uowContext = ctx;
        }
        
        public override Task<int> SaveChangesAsync()
        {
            return _uowContext.SaveChangesAsync();
        }
    }
}