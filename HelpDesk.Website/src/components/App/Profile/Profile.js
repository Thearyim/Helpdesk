import React from 'react';
import { Link } from 'react-router-dom';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import * as Actions from 'SiteStateActions';
import './Profile.css';

const Profile = ({ state, actions }) => {
    
    if (state.session) {
        return (
            <div className="header">
                <nav className="navbar navbar-expand-lg navbar-dark bg-dark static-top">
                    <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNavDropdown" aria-controls="navbarNavDropdown" aria-expanded="false" aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className="collapse navbar-collapse" id="navbarNavDropdown">
                        <ul className="navbar-nav">
                            <li className="nav-item">
                                <Link className="nav-link" to="/Profile">{state.session.username}</Link>
                            </li>
                            <li className="nav-item">
                                <Link className="nav-link" to="/Profile">Log Out</Link>
                            </li>
                        </ul>
                    </div>
                </nav>
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
)(Profile);