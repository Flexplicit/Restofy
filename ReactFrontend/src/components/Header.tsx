import React, { useContext } from 'react'
import { NavLink } from "react-router-dom"
import { AppContext } from '../context/AppContext'

const Header = () => {
    const appState = useContext(AppContext)

    return (
        <header>
            <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
                <div className="container">
                    <NavLink className="navbar-brand" to="/">Restofy</NavLink>
                    <button
                        className="navbar-toggler"
                        type="button"
                        data-toggle="collapse"
                        data-target=".navbar-collapse"
                        aria-controls="navbarSupportedContent"
                        aria-expanded="false"
                        aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                        <ul className="navbar-nav flex-grow-1">
                            <li className="nav-item">
                                <NavLink className="nav-link text-dark" to="/Restaurants">Restaurants</NavLink>
                            </li>
                            <li className="nav-item">
                                <NavLink className="nav-link text-dark" to="/MyRestaurants">My Restaurants</NavLink>
                            </li>
                            <li className="nav-item">
                                <NavLink className="nav-link text-dark" to="/MyOrders">My Orders</NavLink>
                            </li>
                        </ul>
                        {appState.jwt === null ?
                            <ul className="navbar-nav text-dark">
                                <li className="nav-item">
                                    <NavLink className="nav-link text-dark" to="/Identity/Register">Register</NavLink>
                                </li>
                                <li className="nav-item">
                                    <NavLink className="nav-link text-dark" to="/Identity/Login">Login</NavLink>
                                </li>
                            </ul>
                            :
                            <ul className="navbar-nav">
                                <li className="nav-item">
                                    <NavLink className="nav-link text-dark" to="/">Hello {appState.firstName + " " + appState.lastName}!</NavLink>
                                </li>
                                <li>
                                    <NavLink onClick={() => appState.setAuthInfo(null, "", "")} className="nav-item nav-link text-dark" to="/">Logout</NavLink>
                                </li>
                            </ul>
                        }
                    </div>
                </div>
            </nav>
        </header>
    )
}

export default Header
