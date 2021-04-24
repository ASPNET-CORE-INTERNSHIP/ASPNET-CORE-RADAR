## Description

Basic [ASP.NET](http://asp.net/) Core Model-View-Controller (MVC) app built on C# with Aspect Oriented Programming (AOP).

## Features

Features implemented with AOP:
- User authentication
- User authorization with predefined roles and user specific permissions

Rest of the features:
- Session controls 
- Logging of user activity 

## Web API

In order to control the user login attempt, user registration, user authentication, user authorization control and list the user information, HTTP request is made to the ASPNETCORE-WebServer project. The connection is done by predefined urls for both of the solutions. Each request is given a special id consisting of the HttpContext.Session.Id converted to ASCII and shortened.

## Pages

- UserLogin works by asking user for mail and password. If an entry is found with the given mail address and the password matches the one in the database, user is redirected to the profile page. Otherwise, a corresponding error is displayed on the page.
- UserRegister asks  for username, mail and password.
- UserProfile is a restricted page. When user clicks on profile menu on the home page, isAuthenticated Aspect checks whether the current user is logged in. If there is an active cookie, user is admitted. Otherwise, an exception is thrown out.
- UserList requires both isAuthenticated and isAuthorization aspects. Former checks whether the user is logged in while the latter requires admin type user rule. If the requirements are fulfilled, name, mail address and session information for each of users are displayed.
- UserEdit can be accessed only by logged in admin users similar to UserList. This page allows to update the roles of the selected users. 
- AddReceiver page is the first page that user see when the creating process of a radar. In this page user fill the necessary entries to create receiver for new radar. Name is not a necessary entry, if the user does not give a name to receiver, the program gives default name. Then program automatically routes the user to AddAntenna page.
- AddAntenna Page: In creating process user will come to this page twice. One for adding receiver antennas and the other for transmitter antennas. If the user adds multitasking antennas after added the receiver the radar creating process continues with the Monostatic Radar principles. If the user adds multitasking antennas after added the transmitter these antennas will be added normally.
- AddTransmitter Page: In this page user should fill the necessary entries to create a transmitter for current radar. Name is not a necessary entry, if the user does not give a name to transmitter, the program gives default name.
- AddRadar Page: In this page the user will determine radar's general properties.
- AddLocation Page: This page is necessary to keep Radar's location in the database. If the radar is ground radar the user must fill the City, Country, Geographic Latitude and Geographic longitude areas. If the radar belongs to an aircraft the user should fill the airborne area with the name of aircraft.
- AddMode Page: A radar can work with one or multiple aims as Acquisition mode, Search Mode, Tracking mode, Target Illumination Mode, Missile Guidance Mode and more. In AddMode page the user creates a special mode for current Radar. Then the program automatically routes user to AddSubmode Page. After finished all about the mode the user can return AddMode Page to create a new mode for this radar.
- AddSubmode Page: In this page, the user can add different types of submodes to last created Mode. This page is reachable after AddMode Page and AntennaScan (the last page) page.
- AddScan Page: Each submode has their own scan types. After created the submode page the user should specify the submode's scan type.
- AntennaScan Page (Build Relationship Page): In the end of the radar creating process the user should specify which antenna works with which scan type. After this page the user can add new mode, submode or can go to the Done page.
- Done: After created a whole radar and click the "Done!" button in the AntennaScan Page, the program routes user to this page which gives the added radar's ID.
- Admin RadarList Page: This page and its routing pages are reachable only by admin and in this page admin is able to see every radars which have been added before and their basic informations. Admin can select spesific radars to modify and can go to their edit page which has detailed information and spesific edit functions about selected radar.
- Radar Information: In this page Admin can see all informations adout selected radar's transmitter, receiver, location and modes. By clicking the delete button which locates near radar information line admin can remove the whole radar from database. This page contains a group edit buttons in the end of Radar, Transmitter, Receiver and each mode lines. By using related edit buttons admin can reorganize the informations about Radar, Transmitter, Receiver and Modes. In Mode section there is an add button to add new modes to this radar and each mode has its own delete buttons. There is also a antenna list section in the end of this page. Admin can edit antennas and add new antennas to the current radar. 
- Edit Radar Page: In this page the admin can change radar's name, system and configuration.
- Edit Transmitter Page: In this page admin can change radar's transmitter's name, modulation type, max frequency, min frequency and power.
- Edit Receiver Page: In this page admin can change radar's receiver's name, listening time, rest time and recovery time.
- Edit Location Page: When the location of radar changes, admin can record it with using this page.
- Edit Mode Page: In this page the admin can modify mode's informations (its all about mode name). This page contains a submode table which contains submodes working with current mode. Each submode has its own edit and delete buttons.
- Edit Submode Page: In this page the admin can alter current submode's name, PW, PRI, min frequency and max frequency informations. This page also contains scan informations and the list of antennas which used by current submode. In the end of antenna list there is a Add Relation button which routes you to build relationship page that we used in creating process. Admin cannot edit antennas from this page (because one antenna may be use more than one submodes) but can create new relationships between current submode and the radar's other antennas.
- Edit Antenna Page: In this page the admin can change selected antenna's name, type, beamwidths, polarization, number of feed, location, dimensions and duty. 

## Necessary Programs/Libraries

Program was written in C#, therefore, a special environment for the aforementioned language is needed (Visual Studio 2019 was used for the project). Net Core 5.0 was used for the application and no prior version was tested. Finally, in order to store user data, a local SQL server is needed. SQL Server Management Studio (Version 18.8) was used to initialize the database. For the Aspect side, PostSharp was used. 

## How to Install

1. Download the project and open it in Visual Studio 
2. Create a new database, named as AccountDb, in your local SQL Server (Microsoft SQL Server Managment Studio is recommended)
3. Execute the following queries to create necessary tables

    ```java
    CREATE TABLE AccountInfo(
    	UserID INT IDENTITY PRIMARY KEY,
    	Username nvarchar(100),
    	Usermail nvarchar(100) NOT NULL, 
    	Userpassword nvarchar(100) NOT NULL,
    	Userallow nvarchar(100),
    	Userdeny nvarchar(100)
    );

	CREATE TABLE AccountRoles (
    	Roleid INT IDENTITY PRIMARY KEY,
    	Rolename nvarchar(100) NOT NULL,
    	Roleallow nvarchar(100), 
    	Roledeny nvarchar(100)
    );

    CREATE TABLE RoleAllow (
    	ID INT IDENTITY PRIMARY KEY,
    	Roleid INT FOREIGN KEY REFERENCES AccountRoles(Roleid) ON DELETE CASCADE ON UPDATE CASCADE,
    	Roleallow nvarchar(100)
    );

        CREATE TABLE RoleDeny (
    	ID INT IDENTITY PRIMARY KEY,
    	Roleid INT FOREIGN KEY REFERENCES AccountRoles(Roleid) ON DELETE CASCADE ON UPDATE CASCADE,
    	Roledeny nvarchar(100)
    );

    CREATE TABLE AccountSessions (
    	ID INT IDENTITY PRIMARY KEY,
    	UserID INT,
        SessionID nvarchar(255),
    	LoginDate smalldatetime
    );

    CREATE TABLE UserRoles(
    	ID INT IDENTITY PRIMARY KEY,
    	UserID INT FOREIGN KEY REFERENCES AccountInfo(UserID) ON DELETE CASCADE ON UPDATE CASCADE,
    	Roleid INT FOREIGN KEY REFERENCES AccountRoles(Roleid) ON DELETE CASCADE ON UPDATE CASCADE
    );

    CREATE TABLE UserAllow (
    	ID INT IDENTITY PRIMARY KEY,
    	UserID INT FOREIGN KEY REFERENCES AccountInfo(UserID) ON DELETE CASCADE ON UPDATE CASCADE,
    	Userallow nvarchar(100)
    );

    CREATE TABLE UserDeny (
    	ID INT IDENTITY PRIMARY KEY,
    	UserID INT FOREIGN KEY REFERENCES AccountInfo(UserID) ON DELETE CASCADE ON UPDATE CASCADE,
    	Userdeny nvarchar(100)
    );

    INSERT INTO AccountInfo(Username, Usermail, Userpassword) VALUES ('admin', 'admin@admin.com', 'admin');
    INSERT INTO AccountRoles(Rolename) VALUES ('admin');
    INSERT INTO UserRoles(UserID, Roleid) VALUES (1,1);
    INSERT INTO RoleAllow(Roleid, Roleallow) VALUES (1, '8CAD996A-39EA-4B04-AA60-8069C6A665E9');
    INSERT INTO RoleAllow(Roleid, Roleallow) VALUES (1, '45EADA4A-CFB8-46A9-8DDB-5A1ACCC89D2A');
    INSERT INTO RoleAllow(Roleid, Roleallow) VALUES (1, 'CD3EC045-30FC-49C5-BF71-A1109D895FD4');
    INSERT INTO RoleAllow(Roleid, Roleallow) VALUES (1, '1371A9F7-25FC-4EDC-B82B-ADB3CCEE485B');
    INSERT INTO RoleAllow(Roleid, Roleallow) VALUES (1, '54CABEE8-BDED-447D-BBFB-AFB2859797DF');
    ```

4.  Create a new database, named as RADAR, in your local SQL Server
5. Execute the following queries to create necessary tables
````java
    CREATE TABLE Transmitter(
	    ID uniqueidentifier PRIMARY KEY NOT NULL,
	    name nvarchar(100),
	    modulation_type nvarchar(500),
	    max_frequency INT,
	    min_frequency INT,
	    power INT
    );

    CREATE TABLE Receiver(
	    ID uniqueidentifier PRIMARY KEY NOT NULL,
	    name nvarchar(100),
	    listening_time float NOT NULL,
	    rest_time float NOT NULL,
	    recovery_time float NOT NULL
    );

    CREATE TABLE Antenna (
	    ID uniqueidentifier PRIMARY KEY NOT NULL,
	    name nvarchar(100),
	    type nvarchar(50) NOT NULL CHECK (type IN('parabolic', 'cassegrain', 'phased array')),
	    horizontal_beamwidth FLOAT,
	    vertical_beamwidth FLOAT,
	    polarization nvarchar(500) NOT NULL,
	    number_of_feed INT,
	    horizontal_dimension FLOAT,
	    vertical_dimension FLOAT,
	    duty nvarchar(500) NOT NULL,
	    transmitter_id uniqueidentifier FOREIGN KEY REFERENCES Transmitter(ID) ON DELETE CASCADE ON UPDATE CASCADE DEFAULT ('00000000-0000-0000-0000-000000000000'),
	    receiver_id uniqueidentifier FOREIGN KEY REFERENCES Receiver(ID) ON DELETE CASCADE ON UPDATE CASCADE DEFAULT ('00000000-0000-0000-0000-000000000000'),
	    location nvarchar(500)
    );

    CREATE TABLE Scan(
	    ID uniqueidentifier PRIMARY KEY NOT NULL,
	    name nvarchar(500),
	    type nvarchar(500) NOT NULL,
	    main_aspect nvarchar(500) CHECK (main_aspect IN('north', 'west', 'south', 'east', 'changeable')),
	    scan_angle float,
	    scan_rate float,
	    hits_per_scan INT
    );

    CREATE TABLE AntennaScan(
	    antenna_id uniqueidentifier FOREIGN KEY REFERENCES Antenna(ID) ON DELETE CASCADE ON UPDATE CASCADE,
	    scan_id uniqueidentifier FOREIGN KEY REFERENCES Scan(ID) ON DELETE CASCADE ON UPDATE CASCADE,
	    primary key (antenna_id, scan_id)
    );


    CREATE TABLE Location(
	    ID uniqueidentifier PRIMARY KEY NOT NULL,
	    name nvarchar(500),
	    country nvarchar(500),
	    city nvarchar(500),
	    geographic_latitude nvarchar(500),
	    geographic_longitude nvarchar(500),
	    airborne nvarchar(500) DEFAULT 'Non-airborne radar'
    );

    CREATE TABLE Radar(
	    ID uniqueidentifier PRIMARY KEY NOT NULL,
	    name nvarchar(500),
	    system nvarchar(500) NOT NULL CHECK (system IN('early warning', 'missile guidance', 'target tracking', 'target acquisition', 'airborne intercept', 'fire control', 'surface search', 'battlefield surveillance', 'air mapping', 'countermortar', 'ground surveillance', 'man portable' )),
	    configuration nvarchar(500) NOT NULL CHECK (configuration IN('bistatic', 'continious wave', 'doppler', 'fm-cw', 'monopulse', 'passive', 'planar array', 'pulse doppler')),
	    transmitter_id uniqueidentifier FOREIGN KEY REFERENCES Transmitter(ID) ON DELETE CASCADE ON UPDATE CASCADE,
	    receiver_id uniqueidentifier FOREIGN KEY REFERENCES Receiver(ID) ON DELETE CASCADE ON UPDATE CASCADE,
	    location_id uniqueidentifier FOREIGN KEY REFERENCES Location(ID) ON DELETE CASCADE ON UPDATE CASCADE
    );

    CREATE TABLE Mode(
	    ID uniqueidentifier PRIMARY KEY NOT NULL,
	    name nvarchar(500),
	    radar_id uniqueidentifier FOREIGN KEY REFERENCES Radar(ID) ON DELETE CASCADE ON UPDATE CASCADE
    );

    CREATE TABLE Submode(
	    ID uniqueidentifier PRIMARY KEY NOT NULL,
	    name nvarchar(100),
	    mode_id uniqueidentifier FOREIGN KEY REFERENCES Mode(ID) ON DELETE CASCADE ON UPDATE CASCADE,
	    PW float,
	    PRI float,
	    min_frequency INT,
	    max_frequency INT,
	    scan_id uniqueidentifier FOREIGN KEY REFERENCES Scan(ID) ON DELETE CASCADE ON UPDATE CASCADE
    );

    
````
6. Select "Add Connection" after right clicking on "Data Connections" in "Server Explorer" from the view panel of the Visual Studio 
7. Fill the necessary boxes as shown below and select the database created in step 2

    Data Source: Microsoft SQL Server (SqlClient)

    Server Name: Your local SQL Server's name 

8. Copy the connection string from the "properties" for the server added in the previous step in "Data Connections" & paste it to localDatabase in appsettings.json
9. Install Postgres from Package Manager Console under the Tools menu of the Visual Studio (Required for aspects)
10. Install NHibernate from Package manager console using ``PM>>Install-Package NHibernate`` command
11. Click on "Register license" from the PostSharp options in the Extensions dropdown of Visual Studio
12. Install [ASPNETAOP-WebServer](https://github.com/cenkgokturk/ASPNET-CORE-MVC-WEB-SERVER) and open it in a seperate Visual Studio window 
13. Click "Start without debugging" for the ASPNETAOP-WebServer project
14. Do the same thing for ASPNETAOP *
15. Both projects will be opened in your default browser. Unless you want to debug, you don't have to interact with the WebServer 

* If there is an error regarding the Aspects or the SQL Connections, right click on PSerializable attribute in either of the Aspects and select "Install PostSharp.Redist"
