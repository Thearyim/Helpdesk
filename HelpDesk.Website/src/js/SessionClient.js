/*
 * Provides functions for interacting with the Session API.
 * 
 * References:
 * - Fetch Basics
 *   https://javascript.info/fetch-basics
 */
export default class SessionClient {
    //apiUri;

    // param: apiUri
    // The absolute URI to the Session API (ex: https://localhost:5000/api/sessions)
    constructor(uri) {
        if (uri == null) {
            throw "The API URI parameter is required: uri";
        }

        this.apiUri = uri;
    }

    async login(username, password) {
        let targetUri = `${this.apiUri}/api/sessions`;
        let result = {
            data: null,
            error: null,
            status: null
        };

        // Format:
        // POST: https://localhost:5000/api/sessions
        //
        // BODY:
        // {
        //    'username': 'user@codingchallenge.com',
        //    'password': 'passw@rd'
        // }

        console.log(`Login to server: ${targetUri}`);

        try {
            let response = await fetch(targetUri, {
                //mode: 'no-cors',
                method: 'POST',
                body: JSON.stringify({
                    username: username,
                    password: password
                }),
                headers: {
                    'Accept': 'application/json, text/plain',
                    'Content-Type': 'application/json'
                }
            });

            result.status = response.status;
            console.log(`Response: Status ${response.status}-${response.statusText}`);

            if (response.ok) {
                result.data = await response.json().then(json => result.data = json);
            }
            else {
                result.error = `Login request failed (status = ${response.status}). ${await response.text()}`;
            }
        }
        catch (err) {
            console.log(err);
            result.error = err;
        }

        // console.log(`Result: ${JSON.stringify(result)}`);
        return result;
    }
}
