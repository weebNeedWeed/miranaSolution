# Mirana Online Reader

![GitHub last commit (by committer)](https://img.shields.io/badge/1.0.0-8A2BE2)
![GitHub last commit (by committer)](https://img.shields.io/github/last-commit/weebNeedWeed/miranaSolution)
![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/weebNeedWeed/miranaSolution)


A mini Chinese novels reader which was built based on traditional Three-layer architecture with ASP.NET Core and Dependency Injection for loose coupling.

#### Technologies and libraries used in this project:
1. For front-end side:
    - Using React for smooth UI and also some related libraries. i.e: Framer-motion for animation and Tailwindcss for all styling stuffs in the project.
    - Also using axios to facilitate the API callings.
2. For back-end side:
    - Using ASP.NET Core for exposing API to clients.
    - Based on Three-layer architecture with three main layers: Data (DAL), Services (BLL) and WebApp (UI).
    - Using SQL Server for persistence.
3. Cloud Services: 
    - AWS EC2: for deployment.
    - AWS RDS: A storage solution in Production environment.
    - AWS SES: Delivering recovery email to the users.

## How to install

1. Clone the project.
2. Navigate to the src/miranaSolution.API folder and configure change the connection string in appsetings.json:
   ```json
    "ConnectionStrings": {
        "Database": "Server=localhost;Database=MiranaSolution;Trusted_Connection=True;"
    }
   ```
   Also change the other settings if needed.

3. To create the migration and update the database, turn back to the root folder and run the commands below:
    ```console
    dotnet ef migrations add Initialize -s ./src/miranaSolution.API/ -p ./src/miranaSolution.Data/
    dotnet ef database update -s ./src/miranaSolution.API/ -p ./src/miranaSolution.Data/
    ```
   
4. Finally, run the following command to start the API:
    ```console
    dotnet publish -c Release
    dotnet ./bin/Release/net6.0/miranaSolution.API.dll
    ```

#### For front-end:
1. Navigate to the src/frontend/miranaSolution.WebApp folder, then create the ```.env``` file with those contents:
    ```
    VITE_BASE_ADDRESS=<API-Address>
    ```
    Replace ```<API-Address>``` with the API Address when running API above
2. Build the WebApp project:
   ```console
    npm run build
   ```

## How to test

Just simply go to the root/tests/miranaSolution.Services.Tests and run the command:
```console
dotnet test
```

## Some screenshots
![Header](https://github.com/weebNeedWeed/miranaSolution/blob/master/screenshots/header.png?raw=true)

![Home](https://github.com/weebNeedWeed/miranaSolution/blob/master/screenshots/home.png?raw=true)

![Login](https://github.com/weebNeedWeed/miranaSolution/blob/master/screenshots/login.png?raw=true)
