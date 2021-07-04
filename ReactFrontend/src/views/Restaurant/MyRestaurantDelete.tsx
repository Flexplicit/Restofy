import React, { useContext, useEffect, useState } from 'react'
import { NavLink, useHistory, useParams } from 'react-router-dom'
import { AppContext } from '../../context/AppContext'
import { IRestaurant, IRestaurantEdit } from '../../domain/IRestaurant'
import { BaseServices } from '../../services/base-service'




const MyRestaurantColumns = (props: { restaurant: IRestaurant }) => {
    return (
        <div>
            <dl className="row">
                <dt className="col-sm-2">Id</dt>
                <dd className="col-sm-10">
                    {props.restaurant.id}
                </dd>
                <dt className="col-sm-2">Name</dt>
                <dd className="col-sm-10">
                    {props.restaurant.name}
                </dd>
                <dt className="col-sm-2">SalesCount</dt>
                <dd className="col-sm-10">
                    {props.restaurant.salesCount}
                </dd>
                <dt className="col-sm-2">Picture</dt>
                <dd className="col-sm-10">
                    {props.restaurant.picture}
                </dd>
                <dt className="col-sm-2">Description</dt>
                <dd className="col-sm-10">
                    {props.restaurant.description}
                </dd>
            </dl>
        </div>
    )
}



const MyRestaurantDelete = () => {
    const appState = useContext(AppContext)
    const [restaurantState, setRestaurantState] = useState({ id: "", name: "", description: "", restaurantAddress: "", picture: "" } as IRestaurant)
    const [errorMessageState, setErrorMessageState] = useState([] as string[])
    const { id } = useParams<{ id: string }>();
    const history = useHistory();

    const loadRestaurantApi = async () => {
        const dataApi = await BaseServices.getObjectById("/Restaurant", id, appState.jwt)
        if (dataApi.statusCode === 200) {
            setRestaurantState(dataApi.data as IRestaurant);
        }
    }
    useEffect(() => {
        loadRestaurantApi();
    }, [])


    const onClick = async (ev: Event) => {
        const apiRequest = await BaseServices.deleteObjectById("/Restaurant", id, appState.jwt)
        if (apiRequest.statusCode === 204) {
            history.goBack();
        }
    }




    return (
        <>
            <hr />
            <MyRestaurantColumns restaurant={restaurantState} />

            <input type="submit" onClick={(e) => onClick(e.nativeEvent)} value="Delete" className="btn btn-danger" />

            <NavLink to="/MyRestaurants">Back to List</NavLink>
        </>
    )
}

export default MyRestaurantDelete
