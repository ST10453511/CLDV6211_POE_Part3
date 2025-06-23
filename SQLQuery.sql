--TABLE CREATION
CREATE TABLE Venue (
    VenueID INT IDENTITY(1,1) PRIMARY KEY,
    VenueName NVARCHAR(255) NOT NULL,
    Location NVARCHAR(255) NOT NULL,
    Capacity INT NOT NULL CHECK (Capacity > 0),
    ImageUrl NVARCHAR(500) NULL
);

--TABLE CREATION
CREATE TABLE EventType (
	EventTypeID INT IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(100) NOT NULL
);

--TABLE CREATION
CREATE TABLE Event (
    EventID INT IDENTITY(1,1) PRIMARY KEY,
    EventName NVARCHAR(255) NOT NULL,
    EventDate DATETIME NOT NULL,
    Description NVARCHAR(1000) NULL,
    VenueID INT NULL,
	EventTypeID INT NULL,
    FOREIGN KEY (VenueID) REFERENCES Venue(VenueID) ON DELETE SET NULL,
	FOREIGN KEY (EventTypeID) REFERENCES EventType(EventTypeID) ON DELETE SET NULL,
);

--TABLE CREATION
CREATE TABLE Booking (
    BookingID INT IDENTITY(1,1) PRIMARY KEY,
    EventID INT NOT NULL,
    VenueID INT NOT NULL,
    BookingDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (EventID) REFERENCES Event(EventID) ON DELETE CASCADE,
    FOREIGN KEY (VenueID) REFERENCES Venue(VenueID) ON DELETE CASCADE,
	CONSTRAINT UQ_Venue_Event UNIQUE (VenueID, EventID)
);

CREATE UNIQUE INDEX UQ_Venue_Booking ON Booking (VenueID, BookingDate);

--TABLE INSERTION
INSERT INTO Venue (VenueName, Location, Capacity, ImageUrl)
VALUES 
('Grand Hall', 'Johannesburg South', 40, 'https://via.placeholder.com/300x200?text=Grand+Hall'),
('Oceanview Conference Center', 'Johannesburg South', 45, 'https://via.placeholder.com/300x200?text=Oceanview'),
('Skyline Rooftop', 'Johannesburg South', 30, 'https://via.placeholder.com/300x200?text=Skyline'),
('Downtown Event Hub', 'Johannesburg South', 65, 'https://via.placeholder.com/300x200?text=Downtown+Hub'),
('Conference Centre', 'Johannesburg South', 35, 'https://via.placeholder.com/300x200?text=Downtown+Hub');

--TABLE INSERTION
INSERT INTO EventType (Name)
VALUES
('Wedding'),
('Meeting'),
('Party'),
('Function'),
('Conference');

--TABLE INSERTION
INSERT INTO Event (EventName, EventDate, Description, VenueID, EventTypeID)
VALUES 
('Wedding', '2025-09-22 12:00:00', 'Wedding for a cute couple.', 1, 1),
('FNB Meeting', '2025-07-10 14:00:00', 'A meeting for the project managers of FNB Sandton branch.', 2, 2),
('21st Birthday Party', '2025-07-15 10:00:00', 'Party for a man that is turning 21.', 3, 3),
('FNB End Of Year Function', '2025-12-15 09:00:00', 'End of year function party hosted for FNB staff.', 4, 4),
('Book Conference', '2025-06-27 08:00:00', 'Book conference that entails selling books.', 5, 5);

--TABLE INSERTION
INSERT INTO Booking (EventID, VenueID, BookingDate)
VALUES 
(1, 1, '2025-09-22 12:00:00'),
(2, 2, '2025-07-10 14:00:00'),
(3, 3, '2025-07-15 10:00:00'),
(4, 4, '2025-12-15 09:00:00'),
(5, 5, '2025-06-27 08:00:00');

SELECT * FROM Venue;
SELECT * FROM Event;
SELECT * FROM Booking;
SELECT * FROM EventType;

--Drop table Event;
--Drop table Venue;
--Drop table Booking;
--Drop table EventType;