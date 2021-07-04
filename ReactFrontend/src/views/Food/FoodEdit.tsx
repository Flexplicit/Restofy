/* eslint-disable */

import React, { useContext, useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import ErrorMessage from '../../components/ErrorMessage'
import { AppContext } from '../../context/AppContext'
import { ICost } from '../../domain/ICost'
import { IFood, IFoodEdit } from '../../domain/IFood'
import { IFoodGroup } from '../../domain/IFoodGroup'
import { BaseServices } from '../../services/base-service'

const FoodEdit = () => {
    const appState = useContext(AppContext)
    const [foodState, setFoodState] = useState({} as IFoodEdit)
    const [errorMessageState, setErrorMessageState] = useState([] as string[])
    const [foodGroup, setFoodGroup] = useState([] as IFoodGroup[])

    const { id } = useParams<{ id: string }>();

    const loadEditData = async () => {
        const dataApi = await BaseServices.getObjectById<IFoodEdit>("/Food/GetFood", id, appState.jwt);
        console.log(dataApi, "DATA")
        if (dataApi.statusCode === 200) {
            const res = dataApi.data as IFood
            // const dataApiCost = await BaseServices.getObjectById<ICost>("/Cost", res.id, appState.jwt);

            // if (dataApiCost.statusCode === 200) {
            // const resCost = dataApiCost.data as ICost
            // res.costWithVat = resCost.costWithVat;


            // setFoodState({id: res.id,foodName: res.foodName, id: res.id, costWithVat: res.costWithVat});


            // }
            const foodGroupApi = await BaseServices.getAll("/FoodGroup/GetFoodGroups", appState.jwt);
            if (foodGroupApi.statusCode === 200) {
                setFoodGroup(foodGroupApi.data as IFoodGroup[])
            }
        }
    }
    const onBrowsed = () => document.getElementById("file")!.click();
    const OnPictureUpload = (ev: React.ChangeEvent<HTMLInputElement>) => {
        const text: HTMLInputElement = document.getElementById("imagename")! as HTMLInputElement;
        text.placeholder = (ev.target as HTMLInputElement).files![0].name === undefined ? "" : ev.target.files![0].name;
    }
    useEffect(() => {
        loadEditData();
        setFoodState({ ...foodState, restaurantId: id })
    }, [])


    const onNumberChange = (ev: Event): void => {
        const elem = ev.target as HTMLInputElement
        const elemValue = elem.value;
        if (elemValue.includes('-')) {
            elem.value = "";
        } else {
            setFoodState({ ...foodState, costWithVat: parseInt(elemValue) })
        }
    }
    const onClick = async (ev: Event) => {
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
            setFoodState({ ...foodState, picture: base64 as string })

            const resApi = await BaseServices.updateAnObject<IFoodEdit>
                ({ ...foodState, picture: (base64 as string) }, "/Food/Update", id, appState.jwt as string);

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


    return (
        <>
            <h1>Create</h1>
            <h4>Food Item</h4>
            {/* <hr /> */}

            <div className="row">
                <div className="col-md-4">
                    <hr />
                    <ErrorMessage show={errorMessageState.length > 0} errors={errorMessageState} />
                    <div className="form-group">
                        <label className="control-label" htmlFor="Name">Name</label>
                        <input className="form-control"
                            value={foodState.foodName}
                            onChange={e => setFoodState({ ...foodState, foodName: e.target.value })}
                            type="text" id="Name" />
                    </div>
                    <div className="form-group">
                        <label htmlFor="Description">Description</label>
                        <input className="form-control"
                            value={foodState.description}
                            onChange={e => setFoodState({ ...foodState, description: e.target.value })}
                            type="text" id="Description" />
                    </div>

                    <div className="form-group">
                        <label className="control-label" htmlFor="costWithVat">Cost</label>
                        <input className="form-control"
                            value={foodState.costWithVat}
                            onChange={(e) => onNumberChange(e.nativeEvent)} type="number" min="0.01" step="0.01" name="costWithVat" id="costWithVat" />

                    </div>

                    <div className="form-group">
                        <label className="control-label" htmlFor="FoodGroup">Food Group</label>
                        <select onChange={({ target: { value } }) => setFoodState({ ...foodState, foodGroupId: value })} className="form-select form-control" name="FoodGroup" id="FoodGroup">
                            {foodGroup.map((foodGroup) =>
                                <option
                                    {...foodGroup.id === foodState.foodGroupId ? "selected" : ""}
                                    value={foodGroup.id} id={foodGroup.id} key={foodGroup.id} >
                                    {foodGroup.foodGroupType}
                                </option>)}
                        </select>
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
                        <input type="submit" onClick={()=>history.back()} value="Back to list" className="btn btn-info mr-2" />
                        <button type="submit" onClick={(e) => onClick(e.nativeEvent)} className="btn btn-primary">Save</button>

                    </div>
                </div>
            </div>

        </>
    )
}

export default FoodEdit
