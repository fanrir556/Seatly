CREATE TABLE [dbo].[Friends]
(
    [FlowID] INT NOT NULL PRIMARY KEY,
    [UserID] INT NULL,
    [UserName] NVARCHAR(50) NULL,
    [FriendUserName] NVARCHAR(50) NULL,

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



