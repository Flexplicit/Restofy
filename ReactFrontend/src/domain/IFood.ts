export interface IFood extends IFoodEdit {
    costId: string;
    foodGroup: string
}


export interface IFoodEdit extends IFoodCreate {
    id: string;
}

export interface IFoodCreate {
    foodGroupId: string;
    restaurantId: string;
    costWithVat: number;

    foodName: string;
    description: string;
    picture: string;
}
