# Helpdesk Web Application Demo

<div style="color:blue">

#### Demo: Explain Major Components
</div>

##### Data Store
For the sake of the demo, an in-memory data store is used.  This means that the user account and session data/objects as
well as the ticket data/objects are saved in-memory.  The data is not preserved when the API is shutdown.

##### Helpdesk API
A set of API services that enable the website to interact with the backing data store via a REST interface.

* **Account API/Controller**  
  Enables website users to create and manage accounts via a REST interface.

* **Session API/Controller**  
  Enables website users to login/out of the system via a REST interface. Maintains sessions behind the scenes 
  so that users do not have to login until the session is expired.

* **Ticket API/Controller**  
  Enables website users to create, retrieve and manage help tickets in the system via a REST interface.

##### Helpdesk Website
A React/Redux-based website that enables users to create accounts, to login and to manage tickets (create, retrieve). 

<div style="color:blue">

#### Demo: Build the Components
</div>

* Open the HelpDesk-Web-Application solution and build the entire solution

<div style="color:blue">

#### Demo: Show Examples of Using the API Services (e.g. user accounts, sessions, and tickets)
</div>

We will create an account through the Helpdesk API and login to show examples of session management.  We will
also create an example ticket to show example of ticket management.

* **Startup the Demo Environment**  
  * Open a command/cmd prompt, browse to the website location  
    (ex:  C:\Source\CodeChallenge\Microsoft\HelpDesk.Website) 
  
  * Run the following command:
    ```
    PowerShell .\Run-Demo.ps1
    ```

  * Open Chrome, then YARC for the following examples

* **Example: Create and Retrieve an Account**  
```

// Create Account
POST http://localhost:5000/api/accounts

{
  "username": "testuser",
  "password": "secret"
}


// Get Account
GET http://localhost:5000/api/accounts/testuser
```

* **Example: Login and Logout a User**  
```

// Login User
POST http://localhost:5000/api/sessions

{
  "username": "testuser",
  "password": "secret"
}


// Get/Show Session
GET http://localhost:5000/api/sessions/3


// Logout User
DELETE http://localhost:5000/api/sessions/3


// Get/Show Session does not exist
GET http://localhost:5000/api/sessions/3
```

* **Example: Create, Retrieve and Update a Ticket**  
```

// Login User again. You need to copy the token off somewhere because it is required
// to interact with the ticket APIs.
POST http://localhost:5000/api/sessions

{
  "username": "testuser",
  "password": "secret"
}
 

// Create Ticket (put token returned from user login at the end of the URI)
POST http://localhost:5000/api/tickets?token=

{
  "title": "This is a test ticket",
  "description": "The user needs help with an issue she is having",
  "context": null
}


// Update the ticket description (put token returned from user login at the end of the URI)
PUT http://localhost:5000/api/tickets?token=

{
  copy ticket from return HTTP body in POST below, then update the description
}


// Get the ticket (put token returned from user login at the end of the URI)
GET http://localhost:5000/api/tickets/6?token=

```

<div style="color:blue">

#### Demo: Show Logging User in on Website
</div>

When you log a user in, the site will redirect the user to the home page and display tickets for that
user.  Admins will see tickets for all users.

**User:** user1  
**Pass:** secret  

**User:** user2  
**Pass:** secret 

**User:** admin  
**Pass:** secret 
