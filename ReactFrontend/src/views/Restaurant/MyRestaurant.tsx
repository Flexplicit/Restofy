import React, { useContext, useEffect } from 'react'
import { useState } from 'react'
import { AppContext } from '../../context/AppContext'
import { IRestaurant } from '../../domain/IRestaurant'
import { BaseServices } from '../../services/base-service'
import RestaurantRows from "../../components/restaurantComponents/RestaurantRows"
import { NavLink } from 'react-router-dom'






const MyRestaurant = () => {
    const appState = useContext(AppContext)
    const [restaurants, setRestaurants] = useState([] as IRestaurant[])


    const restaurantData = async () => {
        const restaurantsApi = await BaseServices.getAll<IRestaurant>("/Restaurant/MyRestaurants", appState.jwt as string);
        if (restaurantsApi.statusCode === 200) {
            setRestaurants(restaurantsApi.data as IRestaurant[]);
        }


    }

    useEffect(() => {
        restaurantData()
    }, [])

    console.log(restaurants);


    return (
        <>
            <NavLink to="/MyRestaurants/Create"><button type="button" className="btn btn-primary btn-md round hvr-grow">
                <i className="fas fa-plus mr-1"></i>{'\u00A0'}{'\u00A0'}Create new</button>
            </NavLink>

            <div className="d-flex flex-wrap justify-content-center restaurants-wrapper ">
                {restaurants.map((restaurantItem) =>
                    <RestaurantRows key={restaurantItem.id} restaurant={restaurantItem} />)}
            </div>
        </>
    )
}



export default MyRestaurant


