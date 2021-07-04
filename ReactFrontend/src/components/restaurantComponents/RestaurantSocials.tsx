import React from 'react'
import { NavLink } from 'react-router-dom'
import { IContactView } from '../../domain/IContact'

const RestaurantSocials = (props: { social: IContactView }) => {
    return (
            <>
                {(props.social.type === "Facebook") ?
                    <NavLink to={props.social.contactValue} target="popup">
                        <i className="fab fa-facebook-square fa-2x socials hvr-grow"></i>
                    </NavLink>
                    : null
                }
                {(props.social.type === "Instagram") ?
                    <NavLink to={props.social.contactValue} target="popup">
                        <i className="fab fa-instagram fa-2x socials hvr-grow"></i>
                    </NavLink>
                    : null
                }
                {(props.social.type === "Number") ?
                    <>
                        <NavLink to={props.social.contactValue} target="popup">
                            <i onClick={(e) => e.nativeEvent.preventDefault()} className="fas fa-phone-square-alt fa-2x socials hvr-grow"></i>
                        </NavLink>
                        <span className="phone-number text-in-hover">{props.social.contactValue}</span>
                    </>
                    : null}
            </>
    )
}

export default RestaurantSocials
