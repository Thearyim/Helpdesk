/*
    Constants, Enumerations
*/
export const ACTION_TYPES = {
    LOGIN: "LOGIN",
    LOGOUT: "LOGOUT"
};

/*
    Action Creators
*/
export const loginUser = (session) => ({
    type: ACTION_TYPES.LOGIN,
    session: session
});

export const logoutUser = () => ({
    type: ACTION_TYPES.LOGOUT
});

/*
    Action Reducers
*/
export const siteStateReducer = (state = [], action) => {
    let newState = undefined;

    // State Object Format:
    // session: {
    //      user: '',
    //      data: {
    //          id: 1,
    //          userId: 123,
    //          username: 'user@codingchallenge.com',
    //          userRole: 'User',
    //          token: 'E37B6ABF-894B-4E1A-A6CE-C0A1A3881366',
    //          expiration: '2019-07-31T13:45:30.123456Z'
    //      }
    // }

    switch (action.type) {
        case ACTION_TYPES.LOGIN:

            // Replace session with new session
            newState = {
                session: action.session
            };

            break;

        case ACTION_TYPES.LOGOUT:

            // Remove the current session
            newState = {
                session: null
            };

            break;

        default:
            console.log(`Unchanged state`);
            newState = state;
            break;
    }

    // console.log(`New State: ${JSON.stringify(newState)}`);

    return newState;
};