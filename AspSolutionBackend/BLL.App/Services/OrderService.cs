using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.DTO.OrderModels;
using BLL.App.DTO.OrderModels.DbEnums;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;
using EPaymentType = DAL.App.DTO.OrderModels.DbEnums.EPaymentType;
using FoodInOrder = DAL.App.DTO.OrderModels.FoodInOrder;

// ReSharper disable PossibleMultipleEnumeration

namespace BLL.App.Services
{
    public class OrderService :
        BaseEntityService<IAppUnitOfWork, IOrderRepository, BLLAppDTO.OrderModels.Order,
            DALAppDTO.OrderModels.Order, Guid>, IOrderService
    {
        public OrderService(IAppUnitOfWork serviceUow, IOrderRepository serviceRepository,
            IMapper mapper) : base(serviceUow, serviceRepository, new OrderMapper(mapper))
        {
        }


        public async Task<Order?> MakeAnOrderAsync(IEnumerable<Guid> orderItemsId, Guid appUserId,
            Guid paymentTypeId, Guid? creditCardId)
        {
            if (appUserId == default || paymentTypeId == default || orderItemsId.All(x => x == default))
                return null;

            var foodInOrderCollection =
                await ServiceUow.FoodInOrder.GetFoodInOrderCollectionByIdWithExtraInfoAsync(orderItemsId);
            var builtOrder = new Order
            {
                AppUserId = appUserId,
                PaymentTypeId = paymentTypeId,
                CreatedAt = DateTime.Now,
                OrderNumber = new Random().Next(0, int.MaxValue),
                OrderCompletionStatus = EOrderStatus.NotConfirmed,
                RestaurantId = foodInOrderCollection.First().Food!.RestaurantId,
                CreditCardId = creditCardId
            };
            var resOrder = ServiceRepository.Add(Mapper.Map(builtOrder)!);
            builtOrder.Id = resOrder.Id;
            foreach (var foodInOrderItem in foodInOrderCollection)
            {
                foodInOrderItem.Food!.FoodGroup = null;
                foodInOrderItem.Food!.Restaurant = null;
                foodInOrderItem.Food!.Cost = null;
                foodInOrderItem.OrderId = resOrder.Id;
            }

            ServiceUow.FoodInOrder.UpdateRange(foodInOrderCollection);
            await ServiceUow.SaveChangesAsync();
            return builtOrder;
        }

        public async Task<Order?> ConfirmOrderByRestaurantAsync(Guid orderId, int minutesTillReady,
            Guid restaurantWorkerId)
        {
            var order = await ServiceUow.Orders.FirstOrDefaultUserOrRestaurant(orderId, restaurantWorkerId, true, true);

            if (order!.Restaurant!.AppUserId != restaurantWorkerId)
            {
                //TODO: do something bad here
                throw new AuthenticationException("bruh u dont have access for this");
            }

            order!.OrderCompletionStatus = DAL.App.DTO.OrderModels.DbEnums.EOrderStatus.Confirmed;
            switch (order.PaymentType!.Type)
            {
                case EPaymentType.Card:
                    order!.OrderCompletionStatus = DAL.App.DTO.OrderModels.DbEnums.EOrderStatus.Cooking;
                    HelperPay();
                    break;
                case EPaymentType.Cash:
                    order!.OrderCompletionStatus = DAL.App.DTO.OrderModels.DbEnums.EOrderStatus.Cooking;
                    break;
                default:
                    throw new ArgumentException("This should never happen");
            }

            order.ReadyAt = DateTime.Now.AddMinutes(minutesTillReady);
            ServiceRepository.Update(order);
            await ServiceUow.SaveChangesAsync();

            //TODO: dal -> bll needed as well
            var dalResEntity = order;
            var bllReturnEntity = Mapper.Map(order);
            bllReturnEntity!.IsConfirmedByRestaurant = true;
            bllReturnEntity.IsConfirmedByAppUser = true;
            return bllReturnEntity!;
        }

        public new async Task<Order?> FirstOrDefaultAsync(Guid id, Guid userId, bool noTracking)
        {
            var repoQueryResponse = await ServiceUow.Orders
                .FirstOrDefaultAsync(id, userId);
            return Mapper.Map(repoQueryResponse);
        }

        private bool HelperPay()
        {
            return true;
        }

        public async Task<IEnumerable<Order>> GetAllActiveOrdersAsync(Guid userId, bool restaurantOrders = false)
        {
            var queryOrders = await ServiceRepository.GetAllActiveOrdersAsync(userId);
            return DalToBllOrderMapper(queryOrders);
        }

        public async Task<IEnumerable<Order>> GetAllUserOrdersAsync(Guid userId)
        {
            var queryOrders = await ServiceRepository.GetAllUserOrdersAsync(userId);
            return DalToBllOrderMapper(queryOrders);
        }

        public async Task<IEnumerable<Order>> GetAllRestaurantOrdersAsync(Guid userId)
        {
            var queryOrders = await ServiceRepository.GetAllRestaurantOrdersAsync(userId);
            return DalToBllOrderMapper(queryOrders);
        }

        private IEnumerable<Order> DalToBllOrderMapper(IEnumerable<DALAppDTO.OrderModels.Order> orders)
        {
            return orders.Select(x =>
            {
                var mappedBllEntity = Mapper.Map(x);
                foreach (var foodInOrderItem in x.FoodInOrders ?? new List<FoodInOrder>())
                {
                    mappedBllEntity!.OrderTotalCost +=
                        foodInOrderItem.Amount * (foodInOrderItem.Food!.Cost!.CostWithVat);
                }

                if ((int) mappedBllEntity!.OrderCompletionStatus <= (int) EOrderStatus.Confirmed)
                    return mappedBllEntity;
                mappedBllEntity.IsConfirmedByRestaurant = true;
                mappedBllEntity.IsConfirmedByAppUser = true;

                return mappedBllEntity;
            });
        }
    }
}