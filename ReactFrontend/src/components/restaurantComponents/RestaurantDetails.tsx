import { IContactView } from '../../domain/IContact'
import { IRestaurant } from '../../domain/IRestaurant'
import RestaurantSocials from './RestaurantSocials'

const RestaurantDetails = (props: { restaurant: IRestaurant, contacts: IContactView[] }) => {
    return (
        <>
            <div className="restaurantMenu d-flex flex-row flex-wrap align-items-center">
                <img className="image-radius" src={props.restaurant.picture} alt="logo" width="325" height="230" />
                <div className="d-flex flex-column flex-wrap p-3">
                    <div className="p-3 restaurant_name restaurant-header">
                        {props.restaurant.name}
                    </div>
                    <div className="p-3 restaurant-extra-text">{props.restaurant.description}</div>
                    <div className="p-3 restaurant-extra-text">
                        {props.restaurant.restaurantAddress}
                    </div>
                    <div className="p-3 restaurant-extra-text">
                        {props.contacts.map((contact) => <RestaurantSocials social={contact} key={contact.id} />)}
                    </div>
                </div >
            </div>
        </>
    )
}

export default RestaurantDetails
