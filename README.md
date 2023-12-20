# Chirp

Chirp is a web-based application developed in the BDSA course at ITU by Group 7 using .NET.

## How to make _Chirp!_ work locally
To run our Chirp! locally, the first step is to clone our git repository:
```
git clone https://github.com/ITU-BDSA23-GROUP7/Chirp
```

When the repository is clone, make your way into the `Chirp/src/Chirp.Razor` directory:
```
cd Chirp
cd src
cd Chirp.Razor
```
In this directory you need to set up a user secret containing a connection string to an sql-server using the following command:
```
dotnet user-secrets set "AZURE_SQL_CONNECTIONSTRING" "[insert connection string]"
```
Where `[insert connection string]` is replaced with a connection string refering to an sql server.

When the user secret is made, the program can be run from the same directory using one of the two following commands:
```
dotnet run
```
```
dotnet watch
```

When the program is running you can find it in your browser by following the url in your terminal e.g. `localhost:5000`.

## How to run test suite locally
To run our e2e test first download the VSCode extension 'Playwright Test for VSCode', and then enter ">Test: Install Playwright Browsers" into the search bar. When prompted to overwrite the 'playwright.config.ts' file in the terminal select no, by entering 'n'. 
Then open a terminal and run the program.

Open another terminal run the command:
```
dotnet test
```
from the root of our repository, `/Chirp`

## How to access it online

Go to [this website](https://bdsagroup7chirprazor.azurewebsites.net/) to access it hosted by Azure (in case it is still up online).
