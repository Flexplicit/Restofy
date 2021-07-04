export interface IBill {
  id: string;
  orderId: string;
  totalCostWithoutVat: number;
  totalCostWithVat: number;
}
