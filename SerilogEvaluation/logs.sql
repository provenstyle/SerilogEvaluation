SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF OBJECT_ID(N'dbo.Logs', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Logs (
       Id              INT IDENTITY(1,1) NOT NULL,
       LogMessage      NVARCHAR(MAX)     NULL,
       MessageTemplate NVARCHAR(MAX)     NULL,
       LogLevel        NVARCHAR(128)     NULL,
       LogTimeStamp    DATETIMEOFFSET(7) NOT NULL,
       Exception       NVARCHAR(MAX)     NULL,
       Properties      XML               NULL,
       LogEvent        NVARCHAR(MAX)     NULL,
       ApplicationName NVARCHAR(128)     NULL

       CONSTRAINT PK_Logs PRIMARY KEY CLUSTERED(Id ASC)
    )

END;
