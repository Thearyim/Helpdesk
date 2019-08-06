import React from 'react';
import { withRouter } from 'react-router';
import { Link } from 'react-router-dom';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import * as Actions from 'SiteStateActions';
import $ from 'jquery';
import './Login.css';
import { currentSession, sessionClient } from 'SiteSession';

function Login({ state, actions, history }) {

    async function handleLogin(event) {
        event.preventDefault();
        let username = $("#loginUsername").val();
        let password = $("#loginPassword").val();
        let result = await sessionClient.login(username, password);

        if (result.error) {
            alert(result.error);
        }
        else {
            currentSession.session = result.data;
            actions.loginUser(result.data);
            history.push('/');
        }   
    }

    return (
        <div className="loginContainer">
            <form className="form-group" onSubmit={handleLogin}>
                <label htmlFor="loginUsername">Username
                    <input id="loginUsername" type="text" className="form-control input-sm" defaultValue="" />
                </label>
                <br/>
                <label htmlFor="loginPassword">Password
                    <input id="loginPassword" type="password" className="form-control input-sm" defaultValue="" />
                </label>
                <button className="btn btn-primary btn-sm" type="submit">Login</button><Link id="createAccount" to="/account">Create Account</Link>
            </form>
        </div>
    )
}

/*
    React-Redux Integration
    Function connects the state of this component with application redux state
    management. This causes the state specified to be passed to this component
    in standard React 'props'.
 */
const mapStateToProps = (state) => ({
    state: state
});

/*
    React-Redux Integration
    Function connects the actions required by this component with application redux state
    management. This causes the actions specified to be passed to this component
    in standard React 'props'.
 */
const mapDispatchToProps = (dispatch) => ({
    actions: bindActionCreators(Actions, dispatch)
});

/*
    Function connects this component to Redux state management using the
    state and action specifiers provided.
 */
export default connect(
    mapStateToProps,
    mapDispatchToProps
)(withRouter(Login));
