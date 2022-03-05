CREATE TABLE [dbo].[Word]
(
	[Id] INT NOT NULL , 
    [Name] NCHAR(10) NOT NULL, 
    [Win] INT NULL, 
    [Lose] INT NULL, 
    [Score] INT NULL, 
    PRIMARY KEY ([Name])
)
