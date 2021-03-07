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

## Necessary Programs/Libraries

Program was written in C#, therefore, a special environment for the aforementioned language is needed (Visual Studio 2019 was used for the project). Net Core 5.0 was used for the application and no prior version was tested. Finally, in order to store user data, a local SQL server is needed. SQL Server Management Studio (Version 18.8) was used to initialize the database. For the Aspect side, PostSharp was used. 

## How to Install

1. Download the project and open it in Visual Studio 
2. Create a new database, named as AccountDb, in your local SQL Server (Microsoft SQL Server Managment Studio is recommended)
3. Execute the following queries to create necessary tables

    ```java
    CREATE TABLE AccountRoles (
    	Roleid INT IDENTITY PRIMARY KEY,
    	Rolename nvarchar(100) NOT NULL,
    	Roleallow nvarchar(100), 
    	Roledeny nvarchar(100)
    );

    CREATE TABLE AccountInfo(
    	UserID INT IDENTITY PRIMARY KEY,
    	Username nvarchar(100),
    	Usermail nvarchar(100) NOT NULL, 
    	Userpassword nvarchar(100) NOT NULL,
    	Userallow nvarchar(100),
    	Userdeny nvarchar(100)
    );

    CREATE TABLE UserRoles(
    	ID INT IDENTITY PRIMARY KEY,
    	UserID INT FOREIGN KEY REFERENCES AccountInfo(UserID) ON DELETE CASCADE ON UPDATE CASCADE,
    	Roleid INT FOREIGN KEY REFERENCES AccountRoles(Roleid) ON DELETE CASCADE ON UPDATE CASCADE
    );

    CREATE TABLE AccountSessions (
    	ID INT IDENTITY PRIMARY KEY,
    	Usermail nvarchar(100),
    	LoginDate nvarchar(100),
    	IsLoggedIn INT
    );

    INSERT INTO AccountInfo(Username, Usermail, Userpassword) VALUES ('admin', 'admin@admin.com', 'admin');
    INSERT INTO AccountRoles(Rolename, Roleallow) VALUES ('Admin', '45EADA4A-CFB8-46A9-8DDB-5A1ACCC89D2A');
    INSERT INTO AccountRoles(Rolename, Roledeny) VALUES ('RegularUser', '45EADA4A-CFB8-46A9-8DDB-5A1ACCC89D2A');
    INSERT INTO UserRoles(UserID, Roleid) VALUES (1,1);
    ```

4.  Create a new database, named as RADAR, in your local SQL Server
5. Execute the following queries to create necessary tables
````java
     CREATE TABLE Antenna (
	ID uniqueidentifier PRIMARY KEY NOT NULL,
	type nvarchar(50) NOT NULL CHECK (type IN('parabolic', 'cassegrain', 'phased array')),
	horizontal_beamwidth NUMERIC(4,2),
	vertical_beamwidth NUMERIC(4,2),
	polarization nvarchar(500) NOT NULL,
	number_of_feed INT,
	horizontal_dimension NUMERIC(6,2),
	vertical_dimension NUMERIC(6,2)
    );

   CREATE TABLE Receiver(
	ID uniqueidentifier PRIMARY KEY NOT NULL,
	listening_time INT NOT NULL,
	rest_time INT NOT NULL,
	recovery_time INT NOT NULL
    );
    
    CREATE TABLE Mode(
	ID uniqueidentifier PRIMARY KEY NOT NULL,
	mode nvarchar(500) NOT NULL
    );

    CREATE TABLE Submode(
	ID uniqueidentifier PRIMARY KEY NOT NULL,
	PW numeric(4,2) NOT NULL,
	PRI numeric(4,2) NOT NULL,
	PRF numeric(4,2) NOT NULL
    );
    
    CREATE TABLE Transmitter(
	ID uniqueidentifier PRIMARY KEY NOT NULL,
	power INT NOT NULL
    );

   CREATE TABLE Location(
	ID uniqueidentifier PRIMARY KEY NOT NULL,
	country nvarchar(500) NOT NULL,
	city nvarchar(500) NOT NULL,
	geographic_latitude nvarchar(500) NOT NULL,
	geographic_longitude nvarchar(500) NOT NULL
    );

   CREATE TABLE Scan(
	ID uniqueidentifier PRIMARY KEY NOT NULL,
	type nvarchar(500) NOT NULL CHECK (type IN('circular', 'linear', 'unidirectional', 'bidirectional', 'conical', 'palmer-raster', 'palmer', 'palmer-helical', 'track-while')),
    );

  CREATE TABLE Radar(
	ID uniqueidentifier PRIMARY KEY NOT NULL,
	configuration nvarchar(500) NOT NULL CHECK (configuration IN('bistatic', 'continious wave', 'doppler', 'fm-cw', 'monopulse', 'passive', 'planar array', 'pulse doppler')),
	location uniqueidentifier FOREIGN KEY REFERENCES Location(ID) ON DELETE CASCADE ON UPDATE CASCADE
    );
    
    CREATE TABLE RadarScans(
	ID uniqueidentifier PRIMARY KEY NOT NULL,
	radar_id uniqueidentifier FOREIGN KEY REFERENCES Radar(ID) ON DELETE CASCADE ON UPDATE CASCADE,
	scan_id uniqueidentifier FOREIGN KEY REFERENCES Scan(ID) ON DELETE CASCADE ON UPDATE CASCADE
    );

   CREATE TABLE RadarTransmitter(
	ID uniqueidentifier PRIMARY KEY NOT NULL,
	radar_id uniqueidentifier FOREIGN KEY REFERENCES Radar(ID) ON DELETE CASCADE ON UPDATE CASCADE,
	transmitter_id uniqueidentifier FOREIGN KEY REFERENCES Transmitter(ID) ON DELETE CASCADE ON UPDATE CASCADE,
	transmitter_antenna_id uniqueidentifier FOREIGN KEY REFERENCES Antenna(ID) ON DELETE CASCADE ON UPDATE CASCADE,
    );

  CREATE TABLE RadarReceiver(
	ID uniqueidentifier PRIMARY KEY NOT NULL,
	radar_id uniqueidentifier FOREIGN KEY REFERENCES Radar(ID) ON DELETE CASCADE ON UPDATE CASCADE,
	receiver_id uniqueidentifier FOREIGN KEY REFERENCES Receiver(ID) ON DELETE CASCADE ON UPDATE CASCADE,
	receiver_antenna_id uniqueidentifier FOREIGN KEY REFERENCES Antenna(ID) ON DELETE CASCADE ON UPDATE CASCADE
    );
    
    CREATE TABLE RadarMode(
	ID uniqueidentifier PRIMARY KEY NOT NULL,
	radar_id uniqueidentifier FOREIGN KEY REFERENCES Radar(ID) ON DELETE CASCADE ON UPDATE CASCADE,
	mode_id uniqueidentifier FOREIGN KEY REFERENCES Mode(ID) ON DELETE CASCADE ON UPDATE CASCADE,
    );

   CREATE TABLE ModeSubmode(
	ID uniqueidentifier PRIMARY KEY NOT NULL,
	mode_id uniqueidentifier FOREIGN KEY REFERENCES Mode(ID) ON DELETE CASCADE ON UPDATE CASCADE,
	submode_id uniqueidentifier FOREIGN KEY REFERENCES Submode(ID) ON DELETE CASCADE ON UPDATE CASCADE
    );
    
    CREATE TABLE SubmodeScan(
	ID uniqueidentifier PRIMARY KEY NOT NULL,
	submode_id uniqueidentifier FOREIGN KEY REFERENCES Submode(ID) ON DELETE CASCADE ON UPDATE CASCADE,
	scan_id uniqueidentifier FOREIGN KEY REFERENCES Scan(ID) ON DELETE CASCADE ON UPDATE CASCADE
    );

   ALTER TABLE SubmodeScan
     ADD CONSTRAINT uq_SubmodeScan UNIQUE(submode_id, scan_id);

   ALTER TABLE ModeSubmode
     ADD CONSTRAINT uq_ModeSubmode UNIQUE(mode_id, submode_id);
     
   ALTER TABLE RadarReceiver
     ADD CONSTRAINT uq_RadarReceiver UNIQUE(radar_id, receiver_id, receiver_antenna_id);

   ALTER TABLE RadarTransmitter
     ADD CONSTRAINT uq_RadarTransmitter UNIQUE(radar_id, transmitter_id, transmitter_antenna_id);
     
   ALTER TABLE RadarMode
     ADD CONSTRAINT uq_RadarMode UNIQUE(radar_id, mode_id);
     
    CREATE TABLE AntennaScan(
	ID uniqueidentifier PRIMARY KEY NOT NULL FOREIGN KEY REFERENCES Antenna(ID) ON DELETE CASCADE ON UPDATE CASCADE,
	scan_id uniqueidentifier FOREIGN KEY REFERENCES Scan(ID) ON DELETE CASCADE ON UPDATE CASCADE
    );
    
````
6. Select "Add Connection" after right clicking on "Data Connections" in "Server Explorer" from the view panel of the Visual Studio 
7. Fill the necessary boxes as shown below and select the database created in step 2

    Data Source: Microsoft SQL Server (SqlClient)

    Server Name: Your local SQL Server's name 

8. Copy the connection string from the "properties" for the server added in the previous step in "Data Connections" & paste it to localDatabase in appsettings.json
9. Install Postgres from Package Manager Console under the Tools menu of the Visual Studio (Required for aspects)
10. Click on "Register license" from the PostSharp options in the Extensions dropdown of Visual Studio
10. Install [ASPNETAOP-WebServer](https://github.com/cenkgokturk/ASPNET-CORE-MVC-WEB-SERVER) and open it in a seperate Visual Studio window 
11. Click "Start without debugging" for the ASPNETAOP-WebServer project
12. Do the same thing for ASPNETAOP *
13. Both projects will be opened in your default browser. Unless you want to debug, you don't have to interact with the WebServer 

* If there is an error regarding the Aspects or the SQL Connections, right click on PSerializable attribute in either of the Aspects and select "Install PostSharp.Redist"