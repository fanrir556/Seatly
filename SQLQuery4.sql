CREATE TABLE [dbo].[BookingOrders](
 [OrderID] [int] IDENTITY(1,1) NOT NULL,
 [ActivityID] [int] NULL,
 [ActivityName] [nvarchar](100) NULL,
 [WaitingNumber] [int] NULL,
 [UserName] [nvarchar](256) NULL,
 [DateTime] [datetime] NULL,
 [Status] [nvarchar](50) NULL,
 [ActivityBarcode] [nvarchar](6) NULL,
 [Checked] [bit] NULL,)