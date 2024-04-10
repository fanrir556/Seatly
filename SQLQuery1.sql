USE [master]
GO
/****** Object:  Database [Seatly]    Script Date: 2024/3/29 下午 01:33:48 ******/
CREATE DATABASE [Seatly]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Seatly', FILENAME = N'C:\Users\iSpan\AppData\Local\Microsoft\VisualStudio\SSDT\Seatly.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Seatly_log', FILENAME = N'C:\Users\iSpan\AppData\Local\Microsoft\VisualStudio\SSDT\Seatly_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Seatly] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Seatly].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Seatly] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Seatly] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Seatly] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Seatly] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Seatly] SET ARITHABORT OFF 
GO
ALTER DATABASE [Seatly] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Seatly] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Seatly] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Seatly] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Seatly] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Seatly] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Seatly] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Seatly] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Seatly] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Seatly] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Seatly] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Seatly] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Seatly] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Seatly] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Seatly] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Seatly] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Seatly] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Seatly] SET RECOVERY FULL 
GO
ALTER DATABASE [Seatly] SET  MULTI_USER 
GO
ALTER DATABASE [Seatly] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Seatly] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Seatly] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Seatly] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Seatly] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Seatly] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Seatly] SET QUERY_STORE = ON
GO
ALTER DATABASE [Seatly] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Seatly]
GO
/****** Object:  Table [dbo].[BookingOrders]    Script Date: 2024/3/29 下午 01:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookingOrders](
	[OrderID] [int] NOT NULL,
	[WaitingNumber] [int] NULL,
	[WaitingName] [nvarchar](100) NULL,
	[ContactInfo] [nvarchar](100) NULL,
	[RestaurantID] [int] NULL,
	[Date] [date] NULL,
	[PartySize] [int] NULL,
	[Status] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
CREATE TABLE [dbo].[Friends]
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CollectionItems]    Script Date: 2024/3/29 下午 01:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CollectionItems](
	[SerialID] [int] NOT NULL,
	[UserID] [int] NULL,
	[RestaurantID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[SerialID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 2024/3/29 下午 01:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[CommentID] [int] NOT NULL,
	[MemberAccount] [nvarchar](50) NULL,
	[RestaurantAccount] [nvarchar](50) NULL,
	[reContent] [nvarchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[CommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DailyCheckIn]    Script Date: 2024/3/29 下午 01:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DailyCheckIn](
	[ID] [int] NOT NULL,
	[MemberID] [int] NULL,
	[CheckInTime] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GamePoints]    Script Date: 2024/3/29 下午 01:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GamePoints](
	[ID] [int] NOT NULL,
	[MemberID] [int] NULL,
	[PointsDate] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Members]    Script Date: 2024/3/29 下午 01:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Members](
	[MemberID] [int] NOT NULL,
	[MemberAccount] [nvarchar](20) NULL,
	[MemberPassword] [nvarchar](20) NULL,
	[MemberNickname] [nvarchar](10) NULL,
	[MemberName] [nvarchar](30) NULL,
	[Phone] [nvarchar](16) NULL,
	[Email] [nvarchar](50) NULL,
	[CreatedAt] [datetime] NULL,
	[Permission] [int] NULL,
	[Points] [int] NULL,
	[Age] [int] NULL,
	[Birthday] [date] NULL,
	[Sex] [nvarchar](1) NULL,
	[Validation] [bit] NULL,
 CONSTRAINT [PK__Members__0CF04B38AC717DEA] PRIMARY KEY CLUSTERED 
(
	[MemberID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotificationRecords]    Script Date: 2024/3/29 下午 01:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationRecords](
	[NotificationID] [int] NOT NULL,
	[OrderID] [int] NULL,
	[NotificationType] [nvarchar](50) NULL,
	[NotificationContent] [nvarchar](500) NULL,
	[NotificationStatus] [nvarchar](50) NULL,
	[NotificationTimestamp] [datetime] NULL,
	[EmailAddress] [nvarchar](100) NULL,
	[MessageSent] [bit] NULL,
	[MessageContent] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[NotificationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PointStore]    Script Date: 2024/3/29 下午 01:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PointStore](
	[ProductID] [int] NOT NULL,
	[ProductName] [nvarchar](100) NULL,
	[Category] [nvarchar](50) NULL,
	[ProductPrice] [int] NULL,
	[ProductDescription] [nvarchar](500) NULL,
	[ProductImage] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PointTransactions]    Script Date: 2024/3/29 下午 01:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PointTransactions](
	[ID] [int] NOT NULL,
	[MemberID] [int] NULL,
	[ProductID] [int] NULL,
	[TransactionDate] [datetime] NULL,
	[Active] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ratings]    Script Date: 2024/3/29 下午 01:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ratings](
	[RatingID] [int] NOT NULL,
	[MemberAccount] [nvarchar](50) NULL,
	[RestaurantAccount] [nvarchar](50) NULL,
	[CommentTime] [datetime] NULL,
	[TimelinessScore] [int] NULL,
	[TasteScore] [int] NULL,
	[ServiceScore] [int] NULL,
	[EnvironmentScore] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[RatingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reply]    Script Date: 2024/3/29 下午 01:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reply](
	[ReplyID] [int] NOT NULL,
	[ReviewID] [int] NULL,
	[ReplyAccount] [nvarchar](50) NULL,
	[reContent] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ReplyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RestaurantOffers]    Script Date: 2024/3/29 下午 01:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RestaurantOffers](
	[ID] [int] NOT NULL,
	[RestaurantID] [int] NULL,
	[Photo] [nvarchar](200) NULL,
	[Description] [nvarchar](500) NULL,
	[IsActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Restaurants]    Script Date: 2024/3/29 下午 01:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Restaurants](
	[RestaurantID] [int] NOT NULL,
	[RestaurantAccount] [nvarchar](50) NULL,
	[LoginPassword] [nvarchar](50) NULL,
	[RestaurantName] [nvarchar](100) NULL,
	[RestaurantCategory] [nvarchar](50) NULL,
	[RestaurantPhoto] [nvarchar](100) NULL,
	[MenuPhoto] [nvarchar](100) NULL,
	[RestaurantInfo] [nvarchar](100) NULL,
	[Address] [nvarchar](200) NULL,
	[ReservationAvailable] [bit] NULL,
	[WaitAvailable] [bit] NULL,
	[ReservationURL] [nvarchar](200) NULL,
	[DepartmentStoreName] [nvarchar](100) NULL,
	[Hashtag] [nvarchar](100) NULL,
	[email] [nvarchar](50) NULL,
	[phone] [nvarchar](50) NULL,
	[Validation] [bit] NULL,
 CONSTRAINT [PK__Restaura__87454CB580C1ADB7] PRIMARY KEY CLUSTERED 
(
	[RestaurantID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RestaurantTables]    Script Date: 2024/3/29 下午 01:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RestaurantTables](
	[TableID] [int] NOT NULL,
	[RestaurantID] [int] NULL,
	[TableName] [nvarchar](50) NULL,
	[Capacity] [int] NULL,
	[Status] [nvarchar](10) NULL,
	[PartitionName] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[TableID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RestaurantTimes]    Script Date: 2024/3/29 下午 01:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RestaurantTimes](
	[ID] [int] NOT NULL,
	[RestaurantID] [int] NULL,
	[Weekday] [int] NULL,
	[OpeningTime] [time](7) NULL,
	[ClosingTime] [time](7) NULL,
	[LastAdmission] [time](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WaitlistInfo]    Script Date: 2024/3/29 下午 01:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WaitlistInfo](
	[WaitlistID] [int] NOT NULL,
	[RestaurantID] [int] NULL,
	[SmallTableCurrentNumber] [int] NULL,
	[MediumTableCurrentNumber] [int] NULL,
	[LargeTableCurrentNumber] [int] NULL,
	[SmallTableLastWaitingNumber] [int] NULL,
	[MediumTableLastWaitingNumber] [int] NULL,
	[LargeTableLastWaitingNumber] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[WaitlistID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[NotificationRecords]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[BookingOrders] ([OrderID])
GO
ALTER TABLE [dbo].[NotificationRecords]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[BookingOrders] ([OrderID])
GO
USE [master]
GO
ALTER DATABASE [Seatly] SET  READ_WRITE 
GO
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



