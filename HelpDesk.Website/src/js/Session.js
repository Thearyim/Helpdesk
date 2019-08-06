import SessionClient from 'SessionClient';
import TicketClient from 'TicketClient';

export const sessionClient = new SessionClient('http://localhost:5000');
export const ticketClient = new TicketClient('http://localhost:5000');

export class UserSession {

    constructor() {
        this.session = null;
    }
};

export const currentSession = new UserSession();
