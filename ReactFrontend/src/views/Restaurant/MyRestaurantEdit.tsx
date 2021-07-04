/* eslint-disable */
import React, { useContext, useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import ErrorMessage from '../../components/ErrorMessage'
import { AppContext } from '../../context/AppContext'
import { IRestaurantEdit } from '../../domain/IRestaurant'
import { BaseServices } from '../../services/base-service'
// import { BaseServices } from '../../services/base-service-test'

const MyRestaurantEdit = () => {
    const appState = useContext(AppContext)
    const [restaurantState, setRestaurantState] = useState({} as IRestaurantEdit)
    const [errorMessageState, setErrorMessageState] = useState([] as string[])
    const { id } = useParams<{ id: string }>();


    const loadEditApiData = async () => {

        const restaurantApi = await BaseServices.getObjectById("/Restaurant", id, appState.jwt)
        if (restaurantApi.statusCode === 200) {
            setRestaurantState(restaurantApi.data as IRestaurantEdit)
        }
    }

    const onBrowsed = () => document.getElementById("file")!.click();

    const OnPictureUpload = (ev: React.ChangeEvent<HTMLInputElement>) => {
        const text: HTMLInputElement = document.getElementById("imagename")! as HTMLInputElement;
        text.placeholder = (ev.target as HTMLInputElement).files![0].name === undefined ? "" : ev.target.files![0].name;
    }

    const onClick = async (e: Event) => {
        console.log("IN ON CLICK")
        let errors: string[] = []
        const file = document.getElementById("file") as HTMLInputElement;
        if (file.files?.length === 0) {
            errors = ['Image is required'];
            setErrorMessageState(errors);
            return;
        }
        const f = file.files![0];
        var FR = new FileReader();
        FR.readAsDataURL(f), "filereader";

        FR.onloadend = async function (): Promise<void> {
            var base64 = FR.result;
            setRestaurantState({ ...restaurantState, picture: base64 as string })

            const resApi = await BaseServices.updateAnObject<IRestaurantEdit>
                ({ ...restaurantState, picture: base64 as string }, "/Restaurant", id, appState.jwt as string);

            console.log(resApi);
            if (resApi.statusCode === 204) {
                history.back();
            } else {
                console.log(resApi)
            }
            errors = resApi.messages as string[];
            console.log(errors);
        };
    }
    useEffect(() => {
        loadEditApiData()
    }, [])

    return (
        <>
            <h1>Edit</h1>
            <h4>Restaurant</h4>
            {/* <hr /> */}
            <div className="row">
                <div className="col-md-4">
                    <hr />
                    <ErrorMessage show={errorMessageState.length > 0} errors={errorMessageState} />
                    <div className="form-group">
                        <label className="control-label" htmlFor="Name">Name</label>
                        <input className="form-control"
                            onChange={e => setRestaurantState({ ...restaurantState, nameLang: e.target.value })}
                            value={restaurantState.nameLang}
                            type="text" id="Name" />
                    </div>
                    <div className="form-group">
                        <label htmlFor="Address">Address</label>
                        <input className="form-control"
                            onChange={e => setRestaurantState({ ...restaurantState, restaurantAddress: e.target.value })}
                            value={restaurantState.restaurantAddress}
                            type="text" id="Address" />
                    </div>
                    <div className="form-group">
                        <label htmlFor="Description">Description</label>
                        <input className="form-control" onChange={e => setRestaurantState({ ...restaurantState, descriptionLang: e.target.value })}
                            value={restaurantState.descriptionLang}
                            type="text" id="Description" />
                    </div>
                    <div className="form-group t-1">
                        <div className="input-group shadow">
                            <span className="input-group-text px-3 text-muted"><i className="fas fa-image fa-lg"></i></span>
                            <input type="file" id="file" onChange={(e) => OnPictureUpload(e)} name="img[]" className="d-none" />
                            <input type="text" id="imagename" className="form-control form-control" value={restaurantState.picture} readOnly />
                            <button className="browse btn btn-primary px-4" onClick={onBrowsed} type="button">
                                <i className="fas fa-image"></i> Browse</button>
                        </div>
                    </div>
                    <div className="form-group">
                        <button type="submit" onClick={(e) => onClick(e.nativeEvent)} className="btn btn-primary">Save</button>
                    </div>
                </div>
            </div>
        </>
    )
}


export default MyRestaurantEdit
