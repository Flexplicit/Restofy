import { NavLink } from 'react-router-dom'
import { IRestaurant } from '../../domain/IRestaurant'



const RestaurantRows = (props: { restaurant: IRestaurant }) => {
    return (
        <div className="p-3 ">

            {/* <NavLink className="" to="`/Restaurants/Menu/${restaurant.id}`"> */}
            <div className="d-flex flex-column">
                <div className="borders">
                    <div className="inner-shadow">
                        <img className="restaurantImage text-in-hover" src={props.restaurant.picture} alt="logo" width="300" height="200" />
                    </div>
                    <div className="p-2 restaurant_name text-in-hover">{props.restaurant.nameLang}</div>
                    <div className="p-2 text-in-hover">{props.restaurant.descriptionLang}</div>
                    <div className="p-2 text-in-hover">{props.restaurant.restaurantAddress}</div>
                    <div className="p-2">
                        <NavLink to={"/MyRestaurants/Edit/" + props.restaurant.id}>Edit</NavLink> |{'\u00A0'}
                        <NavLink to={"/MyRestaurants/Delete/" + props.restaurant.id}>Delete</NavLink> |{'\u00A0'}
                        <NavLink to={"/MyRestaurants/Menu/" + props.restaurant.id}>Menu</NavLink> |{'\u00A0'}
                        <NavLink to="`/Restaurants/Orders/${restaurant.id}`">Orders</NavLink>
                    </div>
                </div>
            </div>
            {/* </NavLink> */}
        </div>
    )
}

export default RestaurantRows