export interface IContact extends IContactCreate {
  id: string;
}

export interface IContactView extends IContact {
  type: string;
}

export interface IContactCreate {
  contactTypeId: string;
  restaurantId: string | null;
  contactValue: string;
}
