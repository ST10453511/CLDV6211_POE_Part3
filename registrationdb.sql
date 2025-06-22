USE master
IF EXISTS (SELECT * FROM sys.databases WHERE name ='registrationdb')
DROP DATABASE registrationdb
CREATE DATABASE registrationdb

USE registrationdb

--TABLE CREATION
CREATE TABLE Venue (
    VenueId INT PRIMARY KEY IDENTITY(1,1),
    VenueName NVARCHAR(100) NOT NULL,
    Location NVARCHAR(200),
    Capacity INT,
    ImageUrl NVARCHAR(300)
);

--TABLE CREATION
CREATE TABLE Event (
    EventId INT PRIMARY KEY IDENTITY(1,1),
    EventName NVARCHAR(100),
    EventDate DATE,
    Description NVARCHAR(500),
    VenueId INT,
    FOREIGN KEY (VenueId) REFERENCES Venue(VenueId)
);

--TABLE CREATION
CREATE TABLE Booking (
    BookingId INT PRIMARY KEY IDENTITY(1,1),
    EventId INT,
    VenueId INT,
    BookingDate DATE,
    FOREIGN KEY (EventId) REFERENCES Event(EventId),
    FOREIGN KEY (VenueId) REFERENCES Venue(VenueId)
);

--TABLE INSERTION
INSERT INTO Venue (VenueName, Location, Capacity, ImageUrl)
VALUES 
('Grand Hall', '123 Main St, Cityville', 500, 'https://via.placeholder.com/300x200?text=Grand+Hall'),
('Oceanview Conference Center', '456 Coastal Rd, Seaside', 300, 'https://via.placeholder.com/300x200?text=Oceanview'),
('Skyline Rooftop', '789 Sky Ave, Metropolis', 200, 'https://via.placeholder.com/300x200?text=Skyline'),
('Downtown Event Hub', '101 Central Blvd, Urbantown', 400, 'https://via.placeholder.com/300x200?text=Downtown+Hub');

--TABLE INSERTION
INSERT INTO Event (EventName, EventDate, Description, VenueId)
VALUES 
('Tech Conference 2025', '2025-05-20', 'Annual tech event for developers and startups.', 1),
('Wedding Expo', '2025-06-10', 'A showcase of wedding planners, vendors, and venues.', 2),
('Music Fest', '2025-07-15', 'Live performances by indie and pop artists.', 3),
('Startup Pitch Night', '2025-04-25', 'Founders pitch ideas to investors and incubators.', 1);

--TABLE INSERTION
INSERT INTO Booking (EventId, VenueId, BookingDate)
VALUES 
(1, 1, '2025-04-01'),
(2, 2, '2025-04-05'),
(3, 3, '2025-04-10'),
(4, 1, '2025-04-12');

SELECT * FROM Venue;
SELECT * FROM Event;
SELECT * FROM Booking;

--drop table Booking;
--drop table Event;
--drop table Venue;