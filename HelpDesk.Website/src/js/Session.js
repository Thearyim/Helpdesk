import SessionClient from 'SessionClient';

export const sessionClient = new SessionClient('http://localhost:5000');

export class UserSession {

    constructor() {
        this.session = null;
    }
};

export const currentSession = new UserSession();
