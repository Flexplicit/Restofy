/* eslint-disable */
import React, { useContext, useState } from 'react'
import { useHistory } from 'react-router-dom'
import { AppContext } from '../../context/AppContext'
import { IRestaurantCreate } from '../../domain/IRestaurant'
import { AccountService } from '../../services/account-service'
import { BaseServices } from '../../services/base-service'
import { IUserRegister } from '../../types/IAccountTypes/IUserRegister'
import { IResponseMessage } from '../../types/IResponseMessage'
const MyRestaurantCreate = () => {
    const appState = useContext(AppContext)
    const [restaurantState, setRestaurantState] = useState({} as IRestaurantCreate)
    const [errorMessageState, setErrorMessageState] = useState([] as string[])

    const onClick = async (e: Event) => {
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

            const resApi = await BaseServices.addObject<IRestaurantCreate>({ ...restaurantState, picture: base64 } as IRestaurantCreate, "/Restaurant", appState.jwt as string);
            if (resApi.statusCode === 201) {
                history.back();
                return;
            } 



            console.log((resApi.data as unknown as IResponseMessage).errors)
        };

    }


    const onBrowsed = () => document.getElementById("file")!.click();
    const OnPictureUpload = (ev: React.ChangeEvent<HTMLInputElement>) => {
        const text: HTMLInputElement = document.getElementById("imagename")! as HTMLInputElement;
        text.placeholder = (ev.target as HTMLInputElement).files![0].name === undefined ? "" : ev.target.files![0].name;
    }


    return (
        <>
            <h1>Create</h1>
            <h4>Restaurant</h4>
            {/* <hr /> */}

            <div className="row">
                <div className="col-md-4">
                    <hr />
                    {/* <ErrorMessage show={errorMessageState.length > 0} errors={errorMessageState} /> */}
                    <div className="form-group">
                        <label className="control-label" htmlFor="Name">Name</label>
                        <input className="form-control" onChange={e => setRestaurantState({ ...restaurantState, nameLang: e.target.value })} type="text" id="Name" />
                    </div>
                    <div className="form-group">
                        <label htmlFor="Address">Address</label>
                        <input className="form-control" onChange={e => setRestaurantState({ ...restaurantState, restaurantAddress: e.target.value })} type="text" id="Address" />
                    </div>
                    <div className="form-group">
                        <label htmlFor="Description">Description</label>
                        <input className="form-control" onChange={e => setRestaurantState({ ...restaurantState, descriptionLang: e.target.value })} type="text" id="Description" />
                    </div>
                    <div className="form-group t-1">
                        <div className="input-group shadow">
                            <span className="input-group-text px-3 text-muted"><i className="fas fa-image fa-lg"></i></span>
                            <input type="file" id="file" onChange={(e) => OnPictureUpload(e)} name="img[]" className="d-none" />
                            <input type="text" id="imagename" className="form-control form-control" placeholder="Upload Image" />
                            <button className="browse btn btn-primary px-4" onClick={onBrowsed} type="button">
                                <i className="fas fa-image"></i>Browse</button>
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

export default MyRestaurantCreate
