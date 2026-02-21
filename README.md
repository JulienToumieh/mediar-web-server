# Mediar
A simple family & friends media management server built using ASP.NET and PostgreSQL, following a layered architecture and SOLID principles.

## Feautures
- Login/Register + JWT Authentication
- Create, edit, and delete albums
- Upload media (curently images only) to album and delete them
- User permissions restrict certain users from creating, editing, or deleting
- Basic account management (create and delete accounts)

## Screenshots
<img width="720" height="405" alt="Home Page" src="https://github.com/user-attachments/assets/27a37502-2984-4f1e-975b-c6da02f42290" />
<img width="720" height="405" alt="Album List Page" src="https://github.com/user-attachments/assets/141adc98-c01f-4eea-8351-a3309f1b809f" />
<img width="720" height="405" alt="Create Album Form" src="https://github.com/user-attachments/assets/2d770369-85eb-4140-b75f-3aa7faf312bf" />
<img width="720" height="405" alt="Album Page" src="https://github.com/user-attachments/assets/69a299ab-f10d-4e38-a062-7f3b9e3e0af2" />
<img width="720" height="405" alt="Profile Page" src="https://github.com/user-attachments/assets/961f5004-24c4-4d2b-9ab9-0008b416bc0c" />

## Usage
- Create an *Admin* account first
- Profile page:
    - Log out
    - View and delete user accounts
    - Register a new user (only *admin* accounts can create other accounts - for example, the person who set up the server at home creates accounts for their family members and assigns their permissions)
    - Permission roles:
        - **View (default)**: Can only view albums & media
        - **Edit**: View, create, and edit albums & media
        - **Delete**: View, create, edit, and delete albums & media
        - **Admin**: View, create, edit, and delete albums & media + create & delete accounts with permissions
    - The create, edit, and delete buttons are hidden if the currently logged in user does not have the permissions the use them
- The album list page:
    - The + icon opens a form to create an album (info: name, description, and cover image upload)
    - Click on an album to open it
- The album page:
    - Album details section:   
        - The edit icon open a form to edit the album's details (info: name, description, and cover image upload)
        - The trash icon deleted the album and all the associated media
    - Album media section:
        - The + icon add a new media image (upload)
        - The trash icon at the bottom right corner of every image deletes it

## Technical Details
- Uses ADO.NET to execute user account related queries (create, delete, fetch)
- Uses Entity Framework for albums & media (create, delete, fetch)
- Uses browser cookies to store JWT tokens that authenticate users
- Layered architecture:
    - **Models**: Define the entities and their fields (+ their database table structure)
    - **Views**: Used to define and render the user interface of the website
    - **Controllers**: Intermediary between Views and Services - Responsible for handling requests and calling the correct services + returning the respective responses back to the views
    - **Services**: Intermediary between Controllers and Repositories - Handles business logic, authentication, and calls repositories when database access is needed
    - **Repositories**: Intermediary between Services and the Database server - Defines the queries for accessing data from the database (using ADO.NET for user account related queries, or Entity Framework for albums & media related queries)
    - **Database Server**: PostgreSQL server containing all the user data in a structured format

## Setup
### Database (PostgreSQL)
The app requires PostgreSQL database server with the following:
- An superuser account named "postgres" with password "adminpass" - `CREATE ROLE postgres WITH LOGIN SUPERUSER PASSWORD 'adminpass';`
- A database named "mediar" - `CREATE DATABASE mediar;`

#### Additional database scripts (NOT needed if PostgreSQL is installed systemwide)
In the repository, I've included a directory called "_PostgreSQL_DB_Setup" that contains scripts for setting up a portable PostgreSQL installation (this is NOT needed if PostgreSQL is installed systemwide). 

The scripts should be located in the portabe PostgreSQL installation folder, in the same directory as the "bin" filder.


Script order:
1) InitDB.bat (+ follow the comments inside)
2) SetupDB.bat (+ follow the comments inside)

Once the server is set up, you can start and stop it using the following scripts:
- StartServer.bat
- StopServer.bat

### Mediar .NET Project
Upon first installation, you will need to migrate the database using the `dotnet ef database update` command

Once the PostgreSQL server is set up and running, you can execute the Mediar .NET project with Visual Studio (or VSCode with .NET Framework V10), using HTTP as the server protocol.

That's it! I hope you like the project as much as I enjoyed building it :D
