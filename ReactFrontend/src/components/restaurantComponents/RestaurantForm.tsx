
import React from 'react'

const RestaurantForm = () => {
    return (
        <div>
            
        </div>
    )
}

export default RestaurantForm

// import React from 'react'
// import { IRestaurantCreate, IRestaurantEdit } from '../../domain/IRestaurant'

// interface IFormState<T> {
//     restaurantState: T
//     setRestaurantState: React.Dispatch<React.SetStateAction<T>>
// }


// const RestaurantForm = <T extends IRestaurantCreate>(state: { formData: IFormState<T> }) => {

//     return (
//         <>
//             <div className="form-group">
//                 <label className="control-label" htmlFor="Name">Name</label>
//                 <input className="form-control"
//                     onChange={e => state.formData.setRestaurantState({ ...state.formData.restaurantState, name: e.target.value })}
//                     value={state.formData.restaurantState.name} type="text" id="Name" />
//             </div>
//             <div className="form-group">
//                 <label htmlFor="Address">Address</label>
//                 <input className="form-control"
//                     onChange={e => state.formData.setRestaurantState({ ...state.formData.restaurantState, restaurantAddress: e.target.value })}
//                     value={state.formData.restaurantState.name}
//                     type="text" id="Address" />
//             </div>
//             <div className="form-group">
//                 <label htmlFor="Description">Description</label>
//                 <input className="form-control"
//                     onChange={e => state.formData.setRestaurantState({ ...state.formData.restaurantState, description: e.target.value })}
//                     value={state.formData.restaurantState.name}
//                     type="text" id="Description" />
//             </div>
//             <div className="form-group t-1">
//                 <div className="input-group shadow">
//                     <span className="input-group-text px-3 text-muted"><i className="fas fa-image fa-lg"></i></span>
//                     <input type="file" id="file" onChange={(e) => OnPictureUpload(e)} name="img[]" className="d-none" />
//                     <input type="text" id="imagename" className="form-control form-control" placeholder="Upload Image" />
//                     <button className="browse btn btn-primary px-4" onClick={onBrowsed} type="button">
//                         <i className="fas fa-image"></i>Browse</button>
//                 </div>
//             </div>
//         </>
//     )
// }

// export default RestaurantForm
