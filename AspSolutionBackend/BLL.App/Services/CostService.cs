using System;
using AutoMapper;
using BLL.App.DTO.OrderModels;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base.Mappers;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;

namespace BLL.App.Services
{
    public class CostService :
        BaseEntityService<IAppUnitOfWork, ICostRepository, BLLAppDTO.OrderModels.Cost,
            DALAppDTO.OrderModels.Cost, Guid>, ICostService
    {
        protected const decimal Vat = 0.2m;

        public CostService(IAppUnitOfWork serviceUow, ICostRepository serviceRepository,
            IMapper mapper) : base(serviceUow, serviceRepository, new CostMapper(mapper))
        {
        }
        
        public BLLAppDTO.OrderModels.Cost GenerateBllCostFromDecimal(decimal costWithVat)
        {
            return new()
            {
                CostWithoutVat = decimal.Multiply(costWithVat, decimal.Subtract(decimal.One, Vat)),
                Vat = Vat,
                CostWithVat = costWithVat
            };
        }
    }
}