import React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import * as Actions from 'SiteStateActions';
import $ from 'jquery';
import './Account.css';

function Account({ state, actions }) {

    return (
        <div className="accountContainer">
            <form>
                <label htmlFor="username">
                    <input id="accountUsername" name="username" type="text" placeholder="Enter username" />
                </label>
                <br />
                <label htmlFor="password">
                    <input id="accountPassword" name="password" type="password" placeholder="Enter password" />
                </label>
                <br />
                <button type="submit">Create Account</button>
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
)(Account);
