import { IFood } from './IFood';
export interface IFoodInOrder extends IFoodInOrderCreate {
    orderId: string
    id: string;
}

export interface IFoodInOrderCreate {
    foodId: string;
    amount: number;
    price: number 
    food: IFood;
}