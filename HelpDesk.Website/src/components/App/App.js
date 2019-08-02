import React from 'react';
import { Switch, Route } from 'react-router-dom';
import { createStore } from 'redux';
import { Provider } from 'react-redux';
import * as siteState from 'SiteState';
import { siteStateReducer } from 'SiteStateActions';
import HeaderContainer from './Header/HeaderContainer.js';
import HomeContainer from './Home/HomeContainer.js';
import Login from './Account/Login.js';
import Account from './Account/Account.js';

function App() {

    const initialState = siteState.initialState;
    const stateStore = createStore(siteStateReducer, initialState);

    return (
        <div>
            <Provider store={stateStore}>
                <HeaderContainer />
                <Switch>
                    <Route
                        exact path="/"
                        render={(props) => (
                            <HomeContainer>
                            </HomeContainer>
                        )}
                    />
                    <Route
                        exact path="/login"
                        render={(props) => (
                            <Login />
                        )}
                    />
                    <Route
                        exact path="/account"
                        render={(props) => (
                            <Account />
                        )}
                    />
                </Switch>
            </Provider>
        </div>
    );
}

export default App;