/*
 * Provides functions for interacting with the Ticket API.
 * 
 * References:
 * - Fetch Basics
 *   https://javascript.info/fetch-basics
 */
export default class TicketClient {
    //apiUri;

    // param: apiUri
    // The absolute URI to the Ticket API (ex: https://localhost:5000/api/tickets)
    constructor(uri) {
        if (uri == null) {
            throw "The API URI parameter is required: uri";
        }

        this.apiUri = uri;
    }

    async getTickets(session) {
        let targetUri = `${this.apiUri}/api/tickets?token=${session.token}`;
        let result = {
            data: null,
            error: null,
            status: null
        };

        // Format:
        // GET: https://localhost:5000/api/tickets?token=6F03ED9B-901E-4A46-8EF2-4684B6C49F10

        console.log(`Get tickets: ${targetUri}`);

        try {
            let response = await fetch(targetUri, {
                //mode: 'no-cors',
                method: 'GET',
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
                result.error = `Request failed (status = ${response.status}). ${await response.text()}`;
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
