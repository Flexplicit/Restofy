export interface IRestaurant extends IRestaurantEdit {
    id: string;
    salesCount: number;
}

export interface IRestaurantEdit extends IRestaurantCreate {
    id: string;
}

export interface IRestaurantCreate {
    name: string;
    restaurantAddress: string
    picture?: string;
    description?: string;
}
