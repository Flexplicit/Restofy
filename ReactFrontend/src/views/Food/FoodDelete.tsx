import React, { useContext, useEffect, useState } from 'react'
import { useParams, useHistory, NavLink } from 'react-router-dom'
import { AppContext } from '../../context/AppContext'
import { IFood } from '../../domain/IFood'
import { IRestaurant } from '../../domain/IRestaurant'
import { BaseServices } from '../../services/base-service'

const FoodDelete = () => {
    const appState = useContext(AppContext)
    const [foodState, setFoodState] = useState({} as IFood)
    // const [errorMessageState, setErrorMessageState] = useState([] as string[])
    const { id } = useParams<{ id: string }>();
    const history = useHistory();



    const loadFoodApi = async () => {
        const dataApi = await BaseServices.getObjectById("/Food/GetFood", id, appState.jwt)
        if (dataApi.statusCode === 200) {
            setFoodState(dataApi.data as IFood);
        }
    }
    useEffect(() => {
        loadFoodApi();
    }, [])


    const onClick = async (ev: Event) => {
        const apiRequest = await BaseServices.deleteObjectById("/Food/Delete", id, appState.jwt)
        if (apiRequest.statusCode === 204) {
            history.goBack();
        }
    }

    return (
        <div>
            <h1>Deletion</h1>
            <h3>
                Are you sure you want to delete
    <span style={{ fontWeight: "bold" }}> {foodState.foodName}</span>?
            </h3>

            <dl className="row">
                <dt className="col-sm-2">Id</dt>
                <dd className="col-sm-10">
                    {foodState.id}
                </dd>
                <dt className="col-sm-2">Name</dt>
                <dd className="col-sm-10">
                    {foodState.foodName}
                </dd>
                <dt className="col-sm-2">Picture</dt>
                <dd className="col-sm-10">
                    {foodState.picture}
                </dd>
                <dt className="col-sm-2">Description</dt>
                <dd className="col-sm-10">
                    {foodState.description}
                </dd>
                <dt className="col-sm-2">Cost</dt>
                <dd className="col-sm-10">
                    {foodState.costWithVat}
                </dd>
            </dl>

            <input type="submit" onClick={(e) => onClick(e.nativeEvent)} value="Delete" className="btn btn-danger" />

            <NavLink to={"/MyRestaurants/Menu/" + foodState.restaurantId}>Back to List</NavLink>
        </div>
    )
}

export default FoodDelete
