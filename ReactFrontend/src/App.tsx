import React, { useState } from 'react';



import { Route, Switch } from 'react-router-dom';
import Footer from './components/Footer';
import Header from './components/Header';
import { AppContextProvider, initialAppState } from './context/AppContext';
import FoodCreate from './views/Food/FoodCreate';
import FoodDelete from './views/Food/FoodDelete';
import FoodEdit from './views/Food/FoodEdit';
import Login from './views/Identity/Login';
import Register from './views/Identity/Register';
import MyRestaurant from './views/Restaurant/MyRestaurant';
import MyRestaurantCreate from './views/Restaurant/MyRestaurantCreate';
import MyRestaurantDelete from './views/Restaurant/MyRestaurantDelete';
import MyRestaurantEdit from './views/Restaurant/MyRestaurantEdit';
import MyRestaurantMenu from './views/Restaurant/MyRestaurantMenu';
import RestaurantIndex from './views/Restaurant/RestaurantIndex';
import RestaurantMenu from './views/Restaurant/RestaurantMenu';


function App() {
  const setAuthInfo = (jwt: string | null, firstName: string, lastName: string): void => {
    setAppState({ ...appState, jwt, firstName, lastName });
  }
  const [appState, setAppState] = useState({ ...initialAppState, setAuthInfo });



  return (
    <>
      <AppContextProvider value={appState} >
        <Header />
        <div className="container">
          <main role="main" className="pb-3">

            <Switch>
              {/* <Route exact path="/" component={HomeIndex} /> */}


              <Route path="/identity/login" component={Login} />
              <Route path="/identity/register" component={Register} />



              <Route exact={true} path="/Restaurants/Menu/:id" component={RestaurantMenu} />
              <Route exact={true} path="/Restaurants" component={RestaurantIndex} />

              <Route exact={true} path="/Restaurants/Menu/Food/Delete/:id" component={FoodDelete} />
              <Route exact={true} path="/Restaurants/Menu/Food/Edit/:id" component={FoodEdit} />
              <Route exact={true} path="/MyRestaurants/Menu/Food/Create/:id" component={FoodCreate} />

              <Route exact={true} path="/MyRestaurants/Menu/:id" component={MyRestaurantMenu} />
              <Route path="/MyRestaurants/Delete/:id" component={MyRestaurantDelete} />
              <Route path="/MyRestaurants/Edit/:id" component={MyRestaurantEdit} />
              <Route path="/MyRestaurants/Create" component={MyRestaurantCreate} />
              <Route path="/MyRestaurants" component={MyRestaurant} />



              {/* <Route component={Page404} /> */}
            </Switch>
          </main>
        </div>
        <Footer />
      </AppContextProvider>
    </>
  );
}

export default App;
// function setAppState(arg0: any) {
//   throw new Error('Function not implemented.');
// }

