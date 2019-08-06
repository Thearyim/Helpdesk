/*This is the initial Redux state object before the Reducer make changes
 */

// State Object Format:
// {
//  session: {
//      id: 1,
//      userId: 123,
//      username: 'user@codingchallenge.com',
//      userRole: 'User',
//      token: 'E37B6ABF-894B-4E1A-A6CE-C0A1A3881366',
//      expiration: '2019-07-31T13:45:30.123456Z'
//  }
// }
export const initialState = {
    session: null,
    tickets: []
};