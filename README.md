# Countries API/Website
## Running the API
### Prerequisites
- [Git](https://git-scm.com/downloads)
- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download)
### Instructions
1. Clone the repository to your local machine using Git.
2. Open an instance of a terminal (e.g. Windows Command Prompt), and navigate to the root of the repository.
3. To run the tests, execute the command:
    - `dotnet test paymentsense-coding-challenge-api`
4. To run the service, execute the command:       
    - `dotnet watch run --project paymentsense-coding-challenge-api\src\Countries.Api`
5. The last command will open a browser to the Swagger UI of the Countries API where you can execute the endpoint:
    - GET /countries
6. Alternatively you can make API requests with any client of your choice (e.g. Postman) to the URL that the service is running on (this is configured by default to be https://localhost:54786).

## Running the Website
### Prerequisites
- [Git](https://git-scm.com/downloads)
- [Node.js](https://nodejs.org/en/download)
### Instructions
1. Clone the repository to your local machine using Git.
2. Open an instance of a terminal (e.g. Windows Command Prompt), and navigate to the root of the repository.
3. Install the Angular CLI node package by executing the command:
    - `npm install -g @angular/cli`
4. Navigate to the website subfolder by executing the command:
    - `cd paymentsense-coding-challenge-website`
5. Install the node packages for the website by executing the command:
    - `npm install`
6. To run the tests, execute the command:
    - `ng test`
7. To run the website, execute the command:       
    - `ng serve`
8. Open a browser and navigate to http://localhost:4200
