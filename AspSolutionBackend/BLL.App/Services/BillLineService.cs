using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.DTO.OrderModels;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;

namespace BLL.App.Services
{
    public class BillLineService :
        BaseEntityService<IAppUnitOfWork, IBillLinesRepository, BLLAppDTO.OrderModels.BillLine,
            DALAppDTO.OrderModels.BillLine>, IBillLineService
    {
        public BillLineService(IAppUnitOfWork serviceUow, IBillLinesRepository serviceRepository, IMapper mapper) :
            base(serviceUow, serviceRepository, new BillLineMapper(mapper))
        {
        }

        public async Task<IEnumerable<BillLine>> GetAllBillLinesAccordingToBillId(Guid userId, Guid billId)
        {
            return (
                    await ServiceRepository.GetAllBillLinesAccordingToBillId(userId, billId))
                .Select(x => Mapper.Map(x))!;
        }
    }
}