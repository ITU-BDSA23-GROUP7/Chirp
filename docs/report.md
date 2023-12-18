
title: _Chirp!_ Project Report

subtitle: ITU BDSA 2023 Group 7

authors:
- Casper <ccaassppeerr2411@gmail.com>
- Line <linejkpraestegaard@gmail.com>
- Max <max@qbrix.dk>
- Daniel Fich <danielfich2@gmail.com>
- Sebastian <hylandersebastian@gmail.com>

numbersections: true


# Design and Architecture of _Chirp!_

## Domain model

Here comes a description of our domain model.

![Illustration of the _Chirp!_ data model as UML class diagram.](images/BDSA_UML_Onion_architecture.png)

## Architecture — In the small

## Architecture of deployed application

## User activities

### Log in

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

[log-in.md](log-in.md)

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

### automatic build and test

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

### build and deploy to azure
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

## How to run test suite locally

# Ethics

## License

## LLMs, ChatGPT, CoPilot, and others