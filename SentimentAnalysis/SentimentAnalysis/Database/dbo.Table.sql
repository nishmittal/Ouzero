CREATE TABLE [dbo].[Table]
(
    [handle] VARCHAR(100) NOT NULL, 
    [img] VARCHAR(250) NULL, 
    [twitfollow] INT NULL, 
    [twitfriend] INT NULL, 
    [twitretweet] INT NULL, 
    [twitfav] INT NULL, 
    [twitbio] VARCHAR(250) NULL, 
    [twitwebsite] VARCHAR(100) NULL, 
    [alexarank] INT NULL, 
    [alexabounce] INT NULL, 
    [alexpageperf] INT NULL, 
    [alexatraffic] INT NULL, 
    [Category] VARCHAR(40) NULL, 
    [score] INT NULL, 
    PRIMARY KEY ([handle])
)
