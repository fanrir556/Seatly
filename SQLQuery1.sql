    CreatedAt DATETIME,
    Permission INT,
    Points INT
);


CREATE TABLE Restaurants (
    RestaurantID INT PRIMARY KEY,
    RestaurantAccount NVARCHAR(50),
    RestaurantName NVARCHAR(100),
    RestaurantCategory NVARCHAR(50),
	RestaurantPhoto NVARCHAR(100),
	MenuPhoto NVARCHAR(100),
	RestaurantInfo NVARCHAR(100),
    Address NVARCHAR(200),
    ReservationAvailable BIT,
	WaitAvailable BIT,
    ReservationURL NVARCHAR(200),
    DepartmentStoreName NVARCHAR(100),
    Hashtag NVARCHAR(100),
    LoginPassword NVARCHAR(50)
);

CREATE TABLE RestaurantOffers (
    ID INT PRIMARY KEY,
    RestaurantID INT,
    Photo NVARCHAR(200),
    Description NVARCHAR(500),
    IsActive BIT
);

CREATE TABLE RestaurantTimes (
    ID INT PRIMARY KEY,
    RestaurantID INT,
    Weekday INT,
    OpeningTime TIME,
    ClosingTime TIME,
    LastAdmission TIME
);

CREATE TABLE DailyCheckIn (
    ID INT PRIMARY KEY,
    MemberID INT,
    CheckInTime DATE
);

CREATE TABLE GamePoints (
    ID INT PRIMARY KEY,
    MemberID INT,
    PointsDate DATE
);

CREATE TABLE PointStore (
    ProductID INT PRIMARY KEY,
    ProductName NVARCHAR(100),
	Category NVARCHAR(50),
    ProductPrice INT,
    ProductDescription NVARCHAR(500),
    ProductImage NVARCHAR(200)
);

CREATE TABLE PointTransactions (
    ID INT PRIMARY KEY,
    MemberID INT,
    ProductID INT,
    TransactionDate DATETIME,
	Active BIT,
);

CREATE TABLE RestaurantTables (
    TableID INT PRIMARY KEY,
    RestaurantID INT,
    TableName NVARCHAR(50),
    Capacity INT,
    Status NVARCHAR(10),
    PartitionName NVARCHAR(50)
);

CREATE TABLE WaitlistInfo (
    WaitlistID INT PRIMARY KEY,
    RestaurantID INT,
    SmallTableCurrentNumber INT,
    MediumTableCurrentNumber INT,
    LargeTableCurrentNumber INT,
    SmallTableLastWaitingNumber INT,
    MediumTableLastWaitingNumber INT,
    LargeTableLastWaitingNumber INT
);

CREATE TABLE BookingOrders (
    OrderID INT PRIMARY KEY,
    WaitingNumber INT,
    WaitingName NVARCHAR(100),
    ContactInfo NVARCHAR(100),
    RestaurantID INT,
    Date DATE,
    PartySize INT,
    Status NVARCHAR(50)
);

CREATE TABLE NotificationRecords (
    NotificationID INT PRIMARY KEY,
    OrderID INT,
    NotificationType NVARCHAR(50),
    NotificationContent NVARCHAR(500),
    NotificationStatus NVARCHAR(50),
    NotificationTimestamp DATETIME,
    EmailAddress NVARCHAR(100),
    MessageSent BIT,
    MessageContent NVARCHAR(500),
    FOREIGN KEY (OrderID) REFERENCES BookingOrders(OrderID)
);

CREATE TABLE Comments (
    CommentID INT PRIMARY KEY,
    MemberAccount NVARCHAR(50),
    RestaurantAccount NVARCHAR(50),
    reContent NVARCHAR(1000),
);

CREATE TABLE Ratings (
    RatingID INT PRIMARY KEY,
    MemberAccount NVARCHAR(50),
    RestaurantAccount NVARCHAR(50),
    CommentTime DATETIME,
    TimelinessScore INT,
    TasteScore INT,
    ServiceScore INT,
    EnvironmentScore INT,
);

CREATE TABLE CollectionItems (
    SerialID INT PRIMARY KEY,
    UserID INT,
    RestaurantID INT
);

CREATE TABLE Reply (
    ReplyID INT PRIMARY KEY,
    ReviewID INT,
    ReplyAccount NVARCHAR(50),
    Content NVARCHAR(50),
);



    CreatedAt DATETIME,
    Permission INT,
    Points INT
);


CREATE TABLE Restaurants (
    RestaurantID INT PRIMARY KEY,
    RestaurantAccount NVARCHAR(50),
    RestaurantName NVARCHAR(100),
    RestaurantCategory NVARCHAR(50),
	RestaurantPhoto NVARCHAR(100),
	MenuPhoto NVARCHAR(100),
	RestaurantInfo NVARCHAR(100),
    Address NVARCHAR(200),
    ReservationAvailable BIT,
	WaitAvailable BIT,
    ReservationURL NVARCHAR(200),
    DepartmentStoreName NVARCHAR(100),
    Hashtag NVARCHAR(100),
    LoginPassword NVARCHAR(50)
);

CREATE TABLE RestaurantOffers (
    ID INT PRIMARY KEY,
    RestaurantID INT,
    Photo NVARCHAR(200),
    Description NVARCHAR(500),
    IsActive BIT
);

CREATE TABLE RestaurantTimes (
    ID INT PRIMARY KEY,
    RestaurantID INT,
    Weekday INT,
    OpeningTime TIME,
    ClosingTime TIME,
    LastAdmission TIME
);

CREATE TABLE DailyCheckIn (
    ID INT PRIMARY KEY,
    MemberID INT,
    CheckInTime DATE
);

CREATE TABLE GamePoints (
    ID INT PRIMARY KEY,
    MemberID INT,
    PointsDate DATE
);

CREATE TABLE PointStore (
    ProductID INT PRIMARY KEY,
    ProductName NVARCHAR(100),
	Category NVARCHAR(50),
    ProductPrice INT,
    ProductDescription NVARCHAR(500),
    ProductImage NVARCHAR(200)
);

CREATE TABLE PointTransactions (
    ID INT PRIMARY KEY,
    MemberID INT,
    ProductID INT,
    TransactionDate DATETIME,
	Active BIT,
);

CREATE TABLE RestaurantTables (
    TableID INT PRIMARY KEY,
    RestaurantID INT,
    TableName NVARCHAR(50),
    Capacity INT,
    Status NVARCHAR(10),
    PartitionName NVARCHAR(50)
);

CREATE TABLE WaitlistInfo (
    WaitlistID INT PRIMARY KEY,
    RestaurantID INT,
    SmallTableCurrentNumber INT,
    MediumTableCurrentNumber INT,
    LargeTableCurrentNumber INT,
    SmallTableLastWaitingNumber INT,
    MediumTableLastWaitingNumber INT,
    LargeTableLastWaitingNumber INT
);

CREATE TABLE BookingOrders (
    OrderID INT PRIMARY KEY,
    WaitingNumber INT,
    WaitingName NVARCHAR(100),
    ContactInfo NVARCHAR(100),
    RestaurantID INT,
    Date DATE,
    PartySize INT,
    Status NVARCHAR(50)
);

CREATE TABLE NotificationRecords (
    NotificationID INT PRIMARY KEY,
    OrderID INT,
    NotificationType NVARCHAR(50),
    NotificationContent NVARCHAR(500),
    NotificationStatus NVARCHAR(50),
    NotificationTimestamp DATETIME,
    EmailAddress NVARCHAR(100),
    MessageSent BIT,
    MessageContent NVARCHAR(500),
    FOREIGN KEY (OrderID) REFERENCES BookingOrders(OrderID)
);

CREATE TABLE Comments (
    CommentID INT PRIMARY KEY,
    MemberAccount NVARCHAR(50),
    RestaurantAccount NVARCHAR(50),
    reContent NVARCHAR(1000),
);

CREATE TABLE Ratings (
    RatingID INT PRIMARY KEY,
    MemberAccount NVARCHAR(50),
    RestaurantAccount NVARCHAR(50),
    CommentTime DATETIME,
    TimelinessScore INT,
    TasteScore INT,
    ServiceScore INT,
    EnvironmentScore INT,
);

CREATE TABLE CollectionItems (
    SerialID INT PRIMARY KEY,
    UserID INT,
    RestaurantID INT
);

CREATE TABLE Reply (
    ReplyID INT PRIMARY KEY,
    ReviewID INT,
    ReplyAccount NVARCHAR(50),
    Content NVARCHAR(50),
);



