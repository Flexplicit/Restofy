import React, { useContext, useState } from 'react'
import { useHistory } from 'react-router-dom'
import ErrorMessage from '../../components/ErrorMessage'
import { AppContext } from '../../context/AppContext'
import { AccountService } from '../../services/account-service'
import { IUserRegister } from '../../types/IAccountTypes/IUserRegister'
import { IJwtResponse } from '../../types/IJwtResponse'
import { IResponseMessage } from '../../types/IResponseMessage'


const Register = () => {
    const appState = useContext(AppContext)
    const [registerState, setRegisterState] = useState({ email: "", password: "", firstname: "", lastname: "" } as IUserRegister)
    const [errorMessageState, setErrorMessageState] = useState([] as string[])
    const accountService = new AccountService("https://localhost:5001/api/v1/Account");
    let history = useHistory();


    const logInClicked = async (e: Event) => {
        e.preventDefault();
        const errors = Object.keys(registerState).filter(x => registerState[x as keyof IUserRegister].length === 0).map(x =>
            (x as string).charAt(0).toUpperCase() + x.slice(1) + " Field is required.");
        if (errors.length !== 0) {
            setErrorMessageState(errors);
            return;
        }

        const responseRegister = await accountService.register(registerState);
        if (responseRegister.statusCode !== 200) {
            console.log(responseRegister.data)
            setErrorMessageState((responseRegister.data as IResponseMessage).errors)
            return;
        }
        const responseData = responseRegister.data as IJwtResponse;
        appState.setAuthInfo(responseData.token, responseData.firstname, responseData.lastname)
        history.goBack();
    }

    return (
        <>
            <h1>Register a new account</h1>
            <div className="row">
                <div className="col-md-4">
                    <hr />
                    <ErrorMessage show={errorMessageState.length > 0} errors={errorMessageState} />

                    <div className="form-group">
                        <label htmlFor="Input_Email">Email</label>
                        <input className="form-control" onChange={(e) => setRegisterState({ ...registerState, email: e.target.value })} type="email" id="Input_Email" />
                    </div>
                    <div className="form-group">
                        <label htmlFor="Input_Password">Password</label>
                        <input className="form-control" onChange={(e) => setRegisterState({ ...registerState, password: e.target.value })} type="password" id="Input_Password" />
                    </div>
                    <div className="form-group">
                        <label htmlFor="Firstname">Firstname</label>
                        <input className="form-control" onChange={(e) => setRegisterState({ ...registerState, firstname: e.target.value })} type="text" id="Firstname" />
                    </div>
                    <div className="form-group">
                        <label htmlFor="Lastname_Password">Lastname</label>
                        <input className="form-control" onChange={(e) => setRegisterState({ ...registerState, lastname: e.target.value })} type="text" id="Lastname_Password" />
                    </div>

                    <div className="form-group">
                        <button onClick={(e) => logInClicked(e.nativeEvent)} type="submit" className="btn btn-primary">Log in</button>
                    </div>
                </div>
            </div>
        </>
    )
}

export default Register
