import React from 'react';
import { withRouter, Redirect } from 'react-router';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import * as Actions from 'SiteStateActions';
import { currentSession, ticketClient } from 'SiteSession';
import './HomeContainer.css';

// Timer is used to define intervals at which the data will be
// refreshed
var refreshTimer;
var refreshInProgress = false;

function HomeContainer({ state, actions, history }) {

    function redirectIfNotLoggedIn() {
        if (currentSession.session == null) {
            history.push("/login");
        }
    }

    /*
        Function calls the Ticket API to refresh the list of events.
    */
    async function refreshTickets() {
        if (!refreshInProgress) {
            try {
                refreshInProgress = true;
                if (currentSession.session != null) {
                    console.log("Get user tickets...");
                    let result = await ticketClient.getTickets(currentSession.session);

                    if (result.error) {
                        alert(result.error);
                    }
                    else {
                        actions.setTickets(result.data);
                    }
                }
            }
            finally {
                refreshInProgress = false;
            }
        }
    }

    /*
       Function sets the event refresh timer and gets the latest
       tickets from the Ticket API at each interval.
   */
    function setRefresh() {
        console.log(`Refreshing tickets: ${new Date(Date.now()).toLocaleString()}`);

        if (refreshTimer == null) {
            console.log("Create refresh interval.");
            refreshTimer = setInterval(async () => await refreshTickets(), 10000);
        }
    }

    // Begin ticket refresh
    redirectIfNotLoggedIn();
    refreshTickets();
    setRefresh();

    return (
        <div className="ticketContainer">
            {
                state.tickets.map(ticket => {
                    return (
                        <div>
                            <div>{ticket.id}, {ticket.title}</div>
                        </div>
                    );
                })
            }
        </div>
    );
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
)(withRouter(HomeContainer));