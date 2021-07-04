export interface IRestaurant extends IRestaurantEdit {
    id: string;
    salesCount: number;
}

export interface IRestaurantEdit extends IRestaurantCreate {
    id: string;
    nameLangId: string;
    descriptionLangId: string;
}

export interface IRestaurantCreate {
    nameLang: string;
    descriptionLang?: string;
    restaurantAddress: string
    picture?: string;
}
