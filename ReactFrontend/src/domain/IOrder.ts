import { IFoodInOrder } from "./IFoodInOrder";
export interface IOrder {
  id: string;
  createdAt: string;
  billCount: number;
  isConfirmedByAppUser: boolean;
  isConfirmedByRestaurant: boolean;
  orderCompletionStatus: number;
  orderNumber: number;
  paymentTypeId: string;
  restaurantId: string;
  readyAt: string | null;
  creditCardId: string | null;
  orderTotalCost: number;
}

export interface IOrderCreate {
  foodInOrderId: string[];
  paymentTypeId: string;
  creditCardId: string | null;
}

export interface IOrderFood {
  foodInOrder: IFoodInOrder[];
}

export interface IOrderConfirmRestaurant {
  orderId: string;
  MinutesTillReady: number;
}
