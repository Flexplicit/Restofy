import React from 'react'




export interface IErrors {
    show: boolean;
    errors: string[]
}


const ErrorMessage = (errors: IErrors) => {
    console.log(errors)
    return errors.show ?
        <div className="text-danger validation-summary-errors" data-valmsg-summary="true" >
            <ul>
                {errors.errors.map((error, index) => <li key={index}> {error}</li>)}
            </ul>
        </div>
        : null;
}

export default ErrorMessage
