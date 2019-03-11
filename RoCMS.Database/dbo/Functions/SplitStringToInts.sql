CREATE FUNCTION [dbo].[SplitStringToInts] ( @stringToSplit NVARCHAR(MAX) )
RETURNS
 @returnList TABLE ([Val] int)
AS
BEGIN

 DECLARE @name NVARCHAR(255)
 DECLARE @pos INT

 WHILE CHARINDEX(',', @stringToSplit) > 0
 BEGIN
  SELECT @pos  = CHARINDEX(',', @stringToSplit)  
  SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)

  INSERT INTO @returnList 
  SELECT CAST(@name AS INT)

  SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
 END

 INSERT INTO @returnList
  SELECT CAST(@stringToSplit AS INT)

 RETURN
END