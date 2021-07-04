import React from 'react'
import { NavLink } from 'react-router-dom'
import { IFood } from '../../domain/IFood'
import ColoredHorizontalLine from '../tools/ColoredLine'

const FoodRows = (props: { foodGroup: IFood[][], userView: boolean }) => {
    return (
        <>
            {props.foodGroup.map((foodGroup, index) =>
                <div key={index} className="borders-menu">
                    <div className="row d-flex justify-content-center p-1">
                        <h3>{foodGroup[0].foodGroup}s</h3>
                    </div>
                    {foodGroup.map((food) =>
                        <div className="restaurantMenu d-flex flex-row flex-wrap align-items-center">
                            <>

                                <img className="image-radius" src={food.picture} alt="logo" width="260" height="185" />
                                <div className="d-flex flex-column flex-wrap p-3">
                                    <div className="p-3 restaurant_name menu-header">{food.foodName}</div>
                                    <div className="p-3 menu-extra-text">{food.description}</div>
                                    <div className="p-3 menu-extra-text">{food.costWithVat}€</div>
                                    {props.userView === false ? <div className="p-3 menu-extra-text">
                                        <NavLink to={"/Restaurants/Menu/Food/Edit/" + food.id}>Edit</NavLink> | <span> </span>
                                        <NavLink to={"/Restaurants/Menu/Food/Delete/" + food.id}>Delete</NavLink>
                                    </div>
                                        : null}
                                </div>
                                {
                                    props.userView === true ? <div className="ml-auto">
                                        <div className="text-center container-card-button">
                                            <button type="button" className="btn btn-default">Add to Cart</button>
                                            <div className="container-custom-btn">
                                                <div className="quantity">
                                                    <a className="quantity__minus"><span>-</span></a>
                                                    <input name="quantity" id={food.id} type="number" min="0" className="quantity__input" value="0" />
                                                    <a className="quantity__plus"><span>+</span></a>
                                                </div>
                                            </div>
                                            <i className="fa fa-shopping-cart icon-card"></i>
                                        </div>
                                    </div>
                                        : null}
                            </>
                        </div>
                    )}
                    <ColoredHorizontalLine className={"hr-line-menu"} />
                </div>
            )
            }
        </>
    )
}

export default FoodRows
