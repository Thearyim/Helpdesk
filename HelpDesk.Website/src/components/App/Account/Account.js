import React from 'react';
import { withRouter } from 'react-router';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import * as Actions from 'SiteStateActions';
import $ from 'jquery';
import './Account.css';
import { currentSession, sessionClient } from 'SiteSession';

function Account({ state, actions, history }) {

    async function handleAccountCreation(event) {
        event.preventDefault();
        let username = $("#accountUsername").val();
        let password = $("#accountPassword").val();

        console.log("Creating user account...");
        let result = await sessionClient.createAccount(username, password);

        if (result.error) {
            alert(result.error);
        }
        else {
            console.log("Logging in user account...");
            let result = await sessionClient.login(username, password);

            if (result.error) {
                alert(result.error);
            }
            else {
                currentSession.session = result.data;
                actions.loginUser(result.data);

                console.log("User logged in. Redirect to home.");
                history.push('/');
            }
        }
    }

    return (
        <div className="accountContainer">
            <form className="form-group" onSubmit={handleAccountCreation}>
                <label htmlFor="accountUsername">Username
                    <input id="accountUsername" type="text" className="form-control input-sm" defaultValue="" />
                </label>
                <br />
                <label htmlFor="accountPassword">Password
                    <input id="accountPassword" type="password" className="form-control input-sm" defaultValue="" />
                </label>
                <button className="btn btn-primary btn-sm" type="submit">Create Account</button>
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
)(withRouter(Account));
