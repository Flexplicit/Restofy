import { error } from 'jquery'
import React, { useContext, useState } from 'react'
import { useHistory } from 'react-router-dom'
import ErrorMessage from '../../components/ErrorMessage'
import { AppContext } from '../../context/AppContext'
import { AccountService } from '../../services/account-service'
import { IJwtResponse } from '../../types/IJwtResponse'
import { IResponseMessage } from '../../types/IResponseMessage'

const Login = () => {
    const appState = useContext(AppContext)
    const [loginState, setloginState] = useState({ email: 'renesissask@gmail.com', password: 'Testcase1!' })
    const [errorMessageState, setErrorMessageState] = useState([] as string[])
    const accountService = new AccountService("https://localhost:5001/api/v1/Account");
    let history = useHistory();

    const logInClicked = async (e: Event) => {
        e.preventDefault();
        // setErrorMessageState([] as string[]);
        if (loginState.email === '') {
            setErrorMessageState(["Email field is required"])
            return;
        }
        if (loginState.password === '') {
            setErrorMessageState(["Password is required"])
            return;
        }
        const responseLogin = await accountService.login({ email: loginState.email, password: loginState.password });
        if (responseLogin.statusCode !== 200) {
            console.log(responseLogin.data as IResponseMessage)
            setErrorMessageState((responseLogin.data as IResponseMessage).errors)
            return;
        }

        const responseData = responseLogin.data as IJwtResponse;
        appState.setAuthInfo(responseData.token, responseData.firstname, responseData.lastname)
        history.goBack();
    }
    return (
        <>
            <h1>Log in</h1>
            <div className="row">
                <div className="col-md-4">
                    <hr />
                    <ErrorMessage show={errorMessageState.length > 0} errors={errorMessageState} />
                    <div className="form-group">
                        <label htmlFor="Input_Email">Email</label>
                        <input className="form-control" onChange={e => setloginState({ ...loginState, email: e.target.value })} value={loginState.email} type="email" id="Input_Email" />
                    </div>
                    <div className="form-group">
                        <label htmlFor="Input_Password">Password</label>
                        <input className="form-control" onChange={e => setloginState({ ...loginState, password: e.target.value })} value={loginState.password} type="password" id="Input_Password" />
                    </div>
                    <div className="form-group">
                        <button type="submit" onClick={(e) => logInClicked(e.nativeEvent)} className="btn btn-primary">Log in</button>
                    </div>
                </div>
            </div>

        </>
    )
}

export default Login
