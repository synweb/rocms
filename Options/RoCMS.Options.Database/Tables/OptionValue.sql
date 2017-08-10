CREATE TABLE [dbo].[OptionValue]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [OptionKeyId] INT NOT NULL, 
    [Value] NVARCHAR(500) NOT NULL, 
    [CreationDate] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [Moderated] BIT NOT NULL DEFAULT 0, 
    
	CONSTRAINT [FK_OptionValue_OptionKey] FOREIGN KEY ([OptionKeyId]) REFERENCES [OptionKey]([Id]) ON DELETE CASCADE
)
