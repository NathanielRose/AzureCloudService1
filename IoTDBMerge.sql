/****** Object:  UserDefinedTableType [dbo].[LiveDataTableType]    Script Date: 4/6/2015 4:18:25 PM ******/

--Drop sp_InsertEvents storing method
IF OBJECT_ID('sp_InsertEvents') > 0 Drop Procedure [dbo].[dp_InsertEvents]
GO

--Drop Events Table
IF OBJECT_ID('EVENTS') > 0 DROP TABLE [dbo].[Events]
GO

--Drop EventTableType used defined table type
DROP TYPE [dbo].[EventTableType]
GO

--Create EventTableType used-defined table type
CREATE TYPE [dbo].[EventTableType] AS TABLE(
	[EventId] [int] NOT NULL,
	[TimeStamp] [nvarchar](100) NOT NULL,
	[EventMessage] [nvarchar](100) NULL
)
GO
/****** Object:  Table [dbo].[LiveData]    Script Date: 4/6/2015 4:18:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Create Events Table
CREATE TABLE [dbo].[Events](
	[EventId] [int] NOT NULL,
	[TimeStamp] [nvarchar](100) NOT NULL,
	[EventMessage] [nvarchar](100) NULL

PRIMARY KEY CLUSTERED 
(
	[EventId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF,
		 IGNORE_DUP_KEY = OFF,
		 ALLOW_ROW_LOCKS = ON,
		 ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[InsertLiveDataRecords]    Script Date: 4/6/2015 4:18:25 PM ******/

CREATE PROCEDURE [dbo].[InsertEvents]

      @Events dbo.EventTableType readonly
AS
BEGIN
	
	MERGE INTO dbo.[EVENTS] AS A
	USING(
		SELECT * FROM @Events
		)

		B ON (A.EventId = B.EventId) 
    WHEN MATCHED THEN 
        UPDATE SET A.DeviceId = B.DeviceId, 
                   A.Value = B.Value, 
                   A.Timestamp = B.Timestamp 
    WHEN NOT MATCHED THEN 
        INSERT ([EventId], [TimeStamp], [EventMessage])  
        VALUES(B.[EventId], B.[TimeStamp], B.[EventMessage]); 
END 
GO

    