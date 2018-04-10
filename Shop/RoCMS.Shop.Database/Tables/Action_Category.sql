CREATE TABLE [Shop].[Action_Category]
(
	[ActionId] INT NOT NULL , 
    [CategoryId] INT NOT NULL
    PRIMARY KEY ([CategoryId], [ActionId]),
	CONSTRAINT [FK_ActionCategory_Action] FOREIGN KEY ([ActionId]) REFERENCES [Shop].[Action] ([HeartId]) ON DELETE CASCADE,
    CONSTRAINT [FK_ActionCategory_Category] FOREIGN KEY ([CategoryId]) REFERENCES [Shop].[Category] ([HeartId]) ON DELETE CASCADE
)
