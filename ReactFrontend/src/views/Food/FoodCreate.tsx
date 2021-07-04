/* eslint-disable */

import React, { useContext, useEffect, useState } from 'react'
import { useHistory } from 'react-router'
import { useParams } from 'react-router-dom'
import ErrorMessage from '../../components/ErrorMessage'
import { AppContext } from '../../context/AppContext'
import { IFoodCreate } from '../../domain/IFood'
import { IFoodGroup } from '../../domain/IFoodGroup'
import { BaseServices } from '../../services/base-service'
import { IResponseMessage } from '../../types/IResponseMessage'

const FoodCreate = () => {
    const appState = useContext(AppContext)
    const [foodState, setFoodState] = useState({ restaurantId: "", picture: "", foodName: "", description: "", foodGroupId: "", costWithVat: 0 } as IFoodCreate)
    const [foodGroup, setFoodGroup] = useState([] as IFoodGroup[])
    const [errorMessageState, setErrorMessageState] = useState([] as string[])
    const { id } = useParams<{ id: string }>();
    const history = useHistory();

    const loadData = async () => {
        const foodGroupApi = await BaseServices.getAll("/FoodGroup/GetFoodGroups", appState.jwt);
        if (foodGroupApi.statusCode === 200) {
            setFoodGroup(foodGroupApi.data as IFoodGroup[])
        }
    }

    const onBrowsed = () => document.getElementById("file")!.click();
    const OnPictureUpload = (ev: React.ChangeEvent<HTMLInputElement>) => {
        const text: HTMLInputElement = document.getElementById("imagename")! as HTMLInputElement;
        text.placeholder = (ev.target as HTMLInputElement).files![0].name === undefined ? "" : ev.target.files![0].name;
    }
    const onClick = async (ev: Event): Promise<void> => {
        let errors: string[] = []
        const file = document.getElementById("file") as HTMLInputElement;
        if (file.files?.length === 0) {
            errors = ['Image is required'];
            setErrorMessageState(errors);
            return;
        }
        if (foodState.foodName === "" || foodState.description === "") {
            setErrorMessageState(["All fields are required"]);
            return;
        }


        const f = file.files![0];
        var FR = new FileReader();
        FR.readAsDataURL(f), "filereader";

        FR.onloadend = async function (): Promise<void> {
            var base64 = FR.result;
            setFoodState({ ...foodState, picture: base64 as string })

            const resApi = await BaseServices.addObject<IFoodCreate>({ ...foodState, picture: base64 } as IFoodCreate, "/Food/Create", appState.jwt);

            if (resApi.statusCode === 201) {
                history.goBack();
                return;
            }

            const resErrors = ((resApi.data as unknown as IResponseMessage))

            const errorResult = Object.values(resErrors.errors !== undefined ? resErrors.errors : []).flatMap((x) => x)
            setErrorMessageState(errorResult);
        };
    }

    useEffect(() => {
        loadData();
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
                        <input className="form-control" onChange={e => setFoodState({ ...foodState, foodName: e.target.value })} type="text" id="Name" />
                    </div>
                    <div className="form-group">
                        <label htmlFor="Address">Address</label>
                        <input className="form-control" onChange={e => setFoodState({ ...foodState, description: e.target.value })} type="text" id="Address" />
                    </div>

                    <div className="form-group">
                        <label className="control-label" htmlFor="costWithVat">Cost</label>
                        <input className="form-control" onChange={(e) => onNumberChange(e.nativeEvent)} type="number" value={0} min="0.01" step="0.01" name="costWithVat" id="costWithVat" />
                        {/* <input className="form-control" onInput={(e) => e.target.validity.valid || (value = '')} type="number" min="0" name="costWithVat" id="costWithVat" /> */}
                    </div>

                    <div className="form-group">
                        <label className="control-label" htmlFor="FoodGroup">Food Group</label>
                        <select onChange={({ target: { value } }) => setFoodState({ ...foodState, foodGroupId: value })} className="form-select form-control" name="FoodGroup" id="FoodGroup">
                            <option hidden selected>Choose food group</option>
                            {foodGroup.map((foodGroup) =>
                                <option value={foodGroup.id} id={foodGroup.id} key={foodGroup.id} >
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
                        <button type="submit" onClick={(e) => onClick(e.nativeEvent)} className="btn btn-primary">Save</button>
                    </div>
                </div>
            </div>

        </>
    )
}

export default FoodCreate
