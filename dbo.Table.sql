CREATE TABLE [dbo].[Word]
(
	[Id] INT NOT NULL , 
    [Name] TEXT NOT NULL, 
    [Win] INT NULL, 
    [Lose] INT NULL, 
    [Score] INT NULL, 
    PRIMARY KEY ([Name])
)
