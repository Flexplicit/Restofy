export interface ICreditCard extends ICreditCardCreate {
  id: string;
}

export interface ICreditCardCreate {
  creditCardInfo: ICreditCardInfo;
}

export interface ICreditCardInfo {
  creditCardNumber: string;
  expiryDate: string;
  cvc: string;
}
