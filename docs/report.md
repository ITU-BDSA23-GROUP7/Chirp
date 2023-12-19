---
title: _Chirp!_ Project Report
subtitle: ITU BDSA 2023 Group 7
authors:
- Casper <ccaassppeerr2411@gmail.com>
- Line <linejkpraestegaard@gmail.com>
- Max <max@qbrix.dk>
- Daniel Fich <danielfich2@gmail.com>
- Sebastian <hylandersebastian@gmail.com>
numbersections: true
---

# Design and Architecture of _Chirp!_

## Domain model
Here comes a description of our domain model.

```mermaid
  classDiagram
  direction LR
      class Cheep{
        +CheepId: Guid
        +Text: string
        +TimeStamp: DateTime
        +ToCheepDTO() CheepDTO
      }

      class Author{
        +AuthorID: Guid
        +Name: string 
        +Email:  string
        +CheepStreak: int
        +Hidden: bool
        +ToAuthorDTO() AuthorDTO
      }

      Author "1" <--> "0..N" Cheep : Cheeps
      Author "<span style='background-color:#2a303c'>0..N</span>" <--> "<span style='background-color:#2a303c'>0..N</span>" Author : Follows
```


## Architecture — In the small

![Illustration of the _Chirp!_ data model as UML class diagram.](images/BDSA_UML.jpg)

Here we have chosen some relevant classes, to show our onion architecture.
The most important part to notice here are that dependencies flow inward, ensuring that the inner layers remain independent of the outer layers.
This Makes sure that we try to hold the coupling low between the layers, and makes it more adaptable to change, because we make sure that the outer layers can change without affecting the inner layers.

## Architecture of deployed application

## User activities


### Log in
The following diagram shows the process of signing into Chirp!
```mermaid
stateDiagram-v2
    state "Public timeline" as public
    state "Public timeline" as public2
    state "Log in" as login
    state if_state <<choice>>

    [*] --> public
    public --> login
    login --> if_state
    if_state --> public : No
    if_state --> public2 : Yes
    public2 --> [*]
```
UML activity diagram

### Follow and unfollow
The following diagram shows the process of following an author, viewing their timeline, viewing their cheeps on your own timeline and unfollowing the author.
```mermaid
stateDiagram-v2
    state "Public timeline" as public
    state "Public timeline" as public2
    state "Authors timeline" as author
    state "Private timeline" as private
    state "Private timeline" as private2

    [*] --> public : logged in
    public --> public2 : follow author
    public2 --> author : click on authors name
    author --> private : go to own timeline
    private --> private2 : unfollow author

    note left of private2
      The same as before, 
      but no longer showing 
      the cheeps of the author
    end note

    private2 --> [*]
```

### Adding a cheepstreak
The following diagram shows the process of checking the scoreboard, writing a cheep on the public timeline, and checking the scoreboard again to check whether your streak has increased or not.
```mermaid
stateDiagram-v2
    state "Public timeline" as public
    state "Scoreboard" as scoreboard
    state "Public timeline" as public2
    state "Public timeline" as public3
    state "Scoreboard" as scoreboard2


    [*] --> public : logged in
    public --> scoreboard : Check scoreboard
    scoreboard --> public2
    public2 --> public3 : Make a cheep
    public3 --> scoreboard2 : Check scoreboard again

    note left of scoreboard2
      If your newest cheep 
      was written
       the day before,
        your streak has
         been increased by '1'
    end note
```

## Sequence of functionality/calls trough _Chirp!_

#### Sequence diagram

```mermaid
sequenceDiagram
    actor user as User
    participant webBrowser as WebBrowser
    participant app as Chirp.Razor
    participant auth as Azure authorization server
    participant db as Database

    user ->>+ webBrowser: Access Chirp
        webBrowser ->>+ app: GET

        app ->>+ auth: User.Identity.IsAuthenticated
        auth -->>- app: Response

        opt User is authenticated
            app -)+ db: UserNameExists()
            db --)- app: Response
            opt Username doesn't exist in db
                app -)+ db: CreateNewAuthor()
                db --)- app: New author
            end
            app -)+ db: GetFollowing()
            db --)- app: IEnumerable<string>
        end

        app ->>+ db: GetCheeps()
        db -->>- app: List<CheepDTO>

        app -->>- webBrowser: Response
    webBrowser -->>- user: Response
```
This sequence diagram shows what happens when a user accesses the web application.

It also shows what happens if the user is authenticated, including what happens if the user hasn't been added to the database yet.

# Process

## Build, test, release, and deployment

### Automatic build and test
Whenever a push is made to main, or a pull request is made, github will build and test our program, to make sure that we do not implement a feature that does not pass all our earlier defined tests.
```mermaid
stateDiagram
  state "Build Ubuntu" as build_ubuntu
  state "Dotnet build" as dotnet_build
  state if_state <<choice>> 
  state "Run test" as Test

  [*] --> build_ubuntu : Push or pull-request on main
  build_ubuntu -->  dotnet_build 
  dotnet_build -->  if_state : Build Complete?

  if_state --> Test : True
  if_state --> [*] : False

  Test --> [*]
```

### Build and deploy to Azure
When a push is made to main, it is automatically deployed to our Azure website. We discussed having the program tested before deploying it to make sure that it would work, but decided not to do that since some of our tests was testing our website directly, which could cause problems.
```mermaid
stateDiagram
  state "Build Ubuntu" as build_ubuntu
  state "Dotnet build" as dotnet_build
  state if_state <<choice>> 
  state "Publish project" as publish
  state "Upload Artifacts" as upload

  state "Build Ubuntu" as build_ubuntu2
  state "Download Artifacts from Build" as download
  state "Deploy to Azure" as deploy
  

  [*] --> build_ubuntu : Push on main
  build_ubuntu -->  dotnet_build : Build Ubuntu for publishing project
  dotnet_build -->  if_state : Build Complete?

  if_state --> publish : True
  if_state --> [*] : False

  publish --> upload

  upload --> build_ubuntu2
  build_ubuntu2 --> download : Build Ubuntu for deployment
  download --> deploy
  deploy --> [*]
```
### Automatic build and release to github
Whenever a tag is pushed with the format v\*.\*.\* it is automatically build, published and released to github, with a zip folder for both windows, macos and linux.
```mermaid

stateDiagram
  state "Build Ubuntu" as build_ubuntu
  state "Dotnet build" as dotnet_build
  state if_state <<choice>> 
  state "Publish project" as publish
  state zip
  state "Delete output directory" as Delete_output_directory
  state "Publish on github" as Publish_on_Github
  

  [*] --> build_ubuntu : Push on tag format[v*.*.*]
  build_ubuntu -->  dotnet_build : Build Ubuntu for publishing project
  dotnet_build -->  if_state : Build Complete?

  if_state --> publish : True
 
  if_state --> [*] : False
  publish --> zip
  note left of publish
  Both Zip and publishing and 
  deletion of output directory is 
  for Windows,  macOS and linux
  end note

  zip --> Delete_output_directory

  Delete_output_directory --> Publish_on_Github
  Publish_on_Github --> [*]
```



## Team work

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
To test our program run the command:
```
dotnet test
```
from the root of our repository, `/Chirp`

# Ethics

## License
We have chosen the MIT Licence for our application. 

## LLMs, ChatGPT, CoPilot, and others
Through the project, we have used LLM's occasionally. Most of all we used ChatGPT to help us understand different new libraries or architechtures that we needed to use. ChatGPT was occasionally used to help us debug our code. It rarely had a direct influence on what we wrote, and has been co authored whenever this occured.

We used CoPilot once to speed up a task, but has never really used it for anything helpful. CoPilot was co authored as well.