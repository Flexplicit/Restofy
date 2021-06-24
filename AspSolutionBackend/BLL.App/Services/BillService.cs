using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class BillService :
        BaseEntityService<IAppUnitOfWork, IBillRepository, BLLAppDTO.OrderModels.Bill,
            DALAppDTO.OrderModels.Bill, Guid>, IBillService
    {
        public BillService(IAppUnitOfWork serviceUow, IBillRepository serviceRepository,
            IMapper mapper) : base(serviceUow, serviceRepository, new BillMapper(mapper))
        {
        }

        public async Task<Bill> GenerateBillAndBillLineForOrder(Guid orderId, Guid userId)
        {
            var order = await ServiceUow.Orders.FirstOrDefaultAsync(orderId, userId);
            var bill = new Bill
            {
                OrderId = order!.Id,
            };

            var billLines = new List<BillLine>();
            foreach (var foodItemGroup in order!.FoodInOrders!)
            {
                bill.TotalCostWithVat += foodItemGroup.Amount * foodItemGroup!.Food!.Cost!.CostWithVat;
                bill.TotalCostWithoutVat += foodItemGroup.Amount * foodItemGroup!.Food!.Cost!.CostWithoutVat;
                var billLine = new BillLine
                {
                    Amount = foodItemGroup.Amount,
                    PiecePrice = foodItemGroup.Food.Cost.CostWithVat,
                    PriceMultipliedWithAmountWithoutVat = foodItemGroup.Amount * foodItemGroup.Food.Cost.CostWithoutVat,
                    PriceMultipliedWithAmountWithVat = foodItemGroup.Amount * foodItemGroup.Food.Cost.CostWithVat,
                    Name = foodItemGroup.Food.FoodName
                };
                billLines.Add(billLine);
            }

            bill.BillLines = billLines;
            ServiceRepository.Add(Mapper.Map(bill)!);
            await ServiceUow.SaveChangesAsync();
            return bill;
        }

        public async Task<Bill?> GetBillAccordingToOrderId(Guid orderId, Guid userId)
        {
            var res = await ServiceRepository.GetBillAccordingToOrderId(orderId, userId);
            return Mapper.Map(res);
        }
    }
}