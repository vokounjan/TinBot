﻿CREATE TABLE [dbo].[Schedulers]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Cron] NVARCHAR(50) NOT NULL
)
