export interface IBillLine {
  id: string;
  billId: string;
  name: string;
  amount: number;
  piecePrice: number;
  priceMultipliedWithAmountWithoutVat: number;
  priceMultipliedWithAmountWithVat: number;
}
