CREATE TABLE [dbo].[Accounts]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL,
	[ConsumerKey] NVARCHAR(50) NOT NULL, 
    [ConsumerSecret] NVARCHAR(50) NOT NULL,
    [AccountTypeId] INT NOT NULL, 
    [SchedulerId] INT NULL, 
    CONSTRAINT [FK_Accounts_AccountTypes] FOREIGN KEY ([AccountTypeId]) REFERENCES [dbo].[AccountTypes],
    CONSTRAINT [FK_Accounts_Schedulers] FOREIGN KEY ([SchedulerId]) REFERENCES [dbo].[Schedulers]
)
