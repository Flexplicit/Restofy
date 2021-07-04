import React, { useContext, useEffect, useState } from 'react'
import { NavLink, useParams } from 'react-router-dom'
import FoodRows from '../../components/foodComponents/FoodRows'
import RestaurantDetails from '../../components/restaurantComponents/RestaurantDetails'
import { AppContext } from '../../context/AppContext'
import { IContactView } from '../../domain/IContact'
import { IFood } from '../../domain/IFood'
import { IRestaurant } from '../../domain/IRestaurant'
import { BaseServices } from '../../services/base-service'

const MyRestaurantMenu = () => {
    const appState = useContext(AppContext)
    const [restaurantState, setRestaurantState] = useState({} as IRestaurant)
    const [contactsState, SetContactsState] = useState([] as IContactView[])
    const [foodState, setFoodState] = useState([] as IFood[][])
    const [errorMessageState, setErrorMessageState] = useState([] as string[])
    const { id } = useParams<{ id: string }>();

    const apiDataLoad = async () => {
        const restaurantApi = await BaseServices.getObjectById<IRestaurant>("/Restaurant", id, appState.jwt);
        if (restaurantApi.statusCode === 200) {
            setRestaurantState(restaurantApi.data as IRestaurant);
            const contactsApi = await BaseServices.getAll<IContactView>("/Contact", appState.jwt, id);
            const foodApi = await BaseServices.getAll<IFood>("/Food/GetFoods", appState.jwt, id)
            if (contactsApi.statusCode === 200) {
                SetContactsState(contactsApi.data as IContactView[])
            }
            if (foodApi.statusCode === 200) {
                setFoodState(groupFoodItemsByFoodGroupType(foodApi.data as IFood[]));
            }
        }
    }
    const groupFoodItemsByFoodGroupType = (foods: IFood[]): IFood[][] => {
        const groupedFood: IFood[][] = [];
        let tmpGroup: IFood[] = [];
        let previousGroup = "";

        foods.forEach(foodElement => {
            if (previousGroup === foodElement.foodGroup) {
                tmpGroup.push(foodElement);
                return;
            }
            if (tmpGroup.length !== 0) {
                groupedFood.push(tmpGroup);
                tmpGroup = [];
                previousGroup = foodElement.foodGroup;
                tmpGroup.push(foodElement);
                return;
            }
            previousGroup = foodElement.foodGroup;
            tmpGroup.push(foodElement);
        });
        if (tmpGroup.length !== 0) {
            groupedFood.push(tmpGroup);
        }
        return groupedFood;
    }

    useEffect(() => {
        apiDataLoad();
    }, [])

    return (
        <div>
            <RestaurantDetails restaurant={restaurantState} contacts={contactsState} />

            <NavLink to={"Food/Create/" + restaurantState.id} >
                <button type="button" className="btn btn-primary btn-md round mt-3">
                    <i className="fas fa-plus mr-1"></i> Add new food</button>
            </NavLink>

            <FoodRows foodGroup={foodState} userView={false} />
        </div>
    )
}

export default MyRestaurantMenu
