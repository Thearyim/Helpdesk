import React from 'react';
import { withRouter } from 'react-router';
import { Link } from 'react-router-dom';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import * as Actions from 'SiteStateActions';
import profileIcon from 'SiteImages/userIcon.png';
import { currentSession, sessionClient } from 'SiteSession';
import './Profile.css';

const Profile = ({ state, actions, history }) => {

    async function handleLogout(event) {
        event.preventDefault();

        console.log("Logging user out...");
        let result = await sessionClient.logout(currentSession.id);

        if (result.error) {
            alert(result.error);
        }
        else {
            currentSession.session = null;
            actions.logoutUser();

            console.log("User logged out. Redirect to login.");
            history.push('/login');
        } 
    }

    if (state.session) {
        return (
            <div className="dropdown profile">
                <button className="btn dropdown-toggle" type="button" data-toggle="dropdown">
                    <img className="profileIcon" src={profileIcon} />&nbsp;<span className="caret">{state.session.username}</span>
                </button>
                <ul className="dropdown-menu">
                    <li><Link className="nav-link" to="/account">Account</Link></li>
                    <li><Link className="nav-link" to="/login" onClick={handleLogout}>Sign Out</Link></li>
                </ul>
            </div> 
        );
    }
    else {
        return (
            <div className="header"></div>
        );
    }
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
)(withRouter(Profile));