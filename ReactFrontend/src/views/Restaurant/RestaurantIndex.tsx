import React, { useContext, useEffect } from 'react'
import { useState } from 'react'
import { NavLink } from 'react-router-dom'
import { AppContext } from '../../context/AppContext'
import { IRestaurant } from '../../domain/IRestaurant'
import { BaseServices } from '../../services/base-service'



const RestaurantRows = (props: { restaurant: IRestaurant }) => {
    return (
        <div className="p-3 hvr-grow">
            <NavLink className="" to={"/Restaurants/Menu/" + props.restaurant.id}>
                <div className="d-flex flex-column">
                    <div className="borders">
                        <img className="restaurantImage text-in-hover" src={props.restaurant.picture} alt="logo" width="300" height="200" />
                        <div className="p-2 restaurant_name text-in-hover">{props.restaurant.nameLang}</div>
                        <div className="p-2 text-in-hover">{props.restaurant.descriptionLang}</div>
                        <div className="p-2 text-in-hover">{props.restaurant.restaurantAddress}</div>
                        {/* <div className="p-2 socials"></div> */}
                    </div>
                </div>
            </NavLink>
        </div>
    )
}



const RestaurantIndex = () => {
    const appState = useContext(AppContext)
    const [restaurants, setRestaurants] = useState([] as IRestaurant[])

    const restaurantData = async () => {
        const restaurantsApi = await BaseServices.getAll<IRestaurant>("/Restaurant");
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
            {appState.jwt !== null ? <button type="button" className="btn btn-primary btn-md round">
                <i className="fas fa-plus mr-1"></i> Add Credit Card</button> : null}
            <div className="d-flex flex-wrap justify-content-center restaurants-wrapper">


                {restaurants.map((restaurantItem) =>
                    <RestaurantRows key={restaurantItem.id} restaurant={restaurantItem} />)}
            </div>
        </>
    )
}



export default RestaurantIndex


